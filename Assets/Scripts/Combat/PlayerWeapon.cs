// Script Attached to Player Weapons

using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class PlayerWeapon : Weapon
{
    float _knockback = 3f;

    public override void OnTriggerEnter(Collider other) {
        if (PlayerController.instance.IsAttacking) {
            // Deal damage
            base.OnTriggerEnter(other);

            // Knockback
            var direction = (transform.position - other.gameObject.transform.position).normalized * -1.0f;
            var agent = other.gameObject.GetComponent<NavMeshAgent>();

            if (agent != null) agent.velocity = direction * _knockback;

        }
    }

    public void SetKnockback(float value) {
        _knockback = value;
    }
}
