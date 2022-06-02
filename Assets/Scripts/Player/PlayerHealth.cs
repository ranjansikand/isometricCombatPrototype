using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private int _health;
    private bool _dead = false;

    public void Damage(int damage) {
        // If not dead, reduce health
        if (_dead) return;
        _health -= damage;

        // check if player died
        if (_health <= 0) Dead();
    }

    void Dead() {
        _dead = true;
        PlayerController.instance.OnDeath();
    }
}
