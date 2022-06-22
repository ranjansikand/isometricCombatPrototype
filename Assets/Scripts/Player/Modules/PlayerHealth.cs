using System.Collections;
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
        PlayerController.maxHealthUpdate += UpdateMaxHealth;
        PlayerInventory.usePotion += Recover;
    }

    public void Damage(int damage) {
        if (_dead || _recovering) return;

        ChangeHealth(-1 * damage);
        CameraManager.GenerateImpulse();
        PlayerController.instance.Animator.Play("Hurt");

        // check if player died
        if (_health <= 0) Dead();
        else StartCoroutine(Recovery());
    }

    void Recover(int recovery) {
        ChangeHealth(recovery);

        // Play recovery effect
    }

    void ChangeHealth(int amount) {
        _health = (int) Mathf.Clamp(_health + amount, 0, _maxHealth);
        onHealthUpdate();
    }

    void Dead() {
        _dead = true;
        onDeath();
    }

    void UpdateMaxHealth(int changeAmount) {
        bool healthIsIncreasing = changeAmount > 0;
        float currentHealthPercentage = (1.0f * _health) / (1.0f * _maxHealth);

        _maxHealth += changeAmount;
        
        _health = (int) Mathf.Clamp(_maxHealth * currentHealthPercentage, 0, _maxHealth);
        
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
