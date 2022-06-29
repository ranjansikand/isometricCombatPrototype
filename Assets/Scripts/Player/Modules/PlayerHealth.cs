using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private const int _startingHealth = 100;
    private int _maxHealth, _baseMaxHealth;
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
        _health = _maxHealth = _baseMaxHealth = _startingHealth;
        PlayerController.maxHealthUpdate += UpdateMaxHealth;
        PlayerStats.vitalityIncrease += UpdateBaseHealth;
        PlayerInventory.useSimplePotion += Recover;
        PlayerInventory.useScalingPotion += RecoverByPercentage;
    }

    public void Damage(int damage) {
        if (_dead || _recovering) return;

        ChangeHealth(-1 * damage);
        if (damage >= 2) CameraManager.GenerateImpulse();
        PlayerController.instance.OnHurt();

        // check if player died
        if (_health <= 0) Dead();
        else StartCoroutine(Recovery());
    }

    void Recover(int recovery) {
        ChangeHealth(recovery);

        // Play recovery effect
    }

    void RecoverByPercentage(int percentage) {
        var amount = (int) 1.0f * percentage * _maxHealth;
        ChangeHealth(amount); 
    }

    void ChangeHealth(int amount) {
        _health = (int) Mathf.Clamp(_health + amount, 0, _maxHealth);
        onHealthUpdate();
    }

    void Dead() {
        _dead = true;
        onDeath();
    }

    void UpdateBaseHealth(int vitality) {
        int diff = _maxHealth - _baseMaxHealth;
        float temp = (1 + ((2.5f * vitality) / 100f)) * _startingHealth;

        _baseMaxHealth = (int) temp;
        UpdateMaxHealth(diff);
    }

    void UpdateMaxHealth(int changeAmount) {
        _maxHealth = _baseMaxHealth + changeAmount;
        Debug.Log(_maxHealth);
        onHealthUpdate();
    }

    IEnumerator Recovery() {
        _recovering = true; 
        yield return _recoveryTime; 
        _recovering = false;
    }
}
