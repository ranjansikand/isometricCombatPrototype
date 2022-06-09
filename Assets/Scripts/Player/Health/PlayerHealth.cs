using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private static int _startingHealth = 100;
    private int _maxHealth;
    private int _health;

    private WaitForSeconds _recoveryTime = new WaitForSeconds(0.75f);
    private bool _dead = false;
    private bool _recovering = false;

    public int Health { get {return _health; }}
    public int MaxHealth { get { return _maxHealth; }}

    public delegate void HealthEvent();
    public static HealthEvent onDeath;
    public static HealthEvent onHealthUpdate;

    void Awake() {
        _health = _maxHealth = _startingHealth;
    }

    public void Damage(int damage) {
        if (_dead || _recovering) return;

        _health -= damage;
        onHealthUpdate();

        // check if player died
        if (_health <= 0) Dead();
        else StartCoroutine(Recovery());
    }

    void Dead() {
        _dead = true;
        onDeath();
    }

    void UpdateMaxHealth(int changeAmount) {
        bool healthIsIncreasing = changeAmount > 0;
        float currentHealthPercentage = _health / _maxHealth;

        _maxHealth += changeAmount;

        // update current health
        if (healthIsIncreasing) {
            // do not want to remove health if it's decreasing
            _health = (int) (_maxHealth * currentHealthPercentage);
        }
        if (_health > _maxHealth) {
            _health = _maxHealth;
        }

        onHealthUpdate();
    }

    IEnumerator Recovery() {
        _recovering = true;
        yield return _recoveryTime;
        _recovering = false;
    }
}
