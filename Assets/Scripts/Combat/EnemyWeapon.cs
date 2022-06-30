using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : Weapon
{
    EnemyStateMachine _context;

    void Awake() {
        _context = GetComponentInParent<EnemyStateMachine>();
    }

    public override void OnTriggerEnter(Collider other) {
        if (_context.IsAttacking) {
            base.OnTriggerEnter(other);
        }
    }
}
