// Script Attached to Player Weapons

using UnityEngine;

[RequireComponent(typeof(Collider))]
public class WeaponScript : MonoBehaviour
{
    static int damage;

    public void SetDamage(int baseDamage) {
        damage = baseDamage;
    }

    public virtual void OnTriggerEnter(Collider other) {
        if (PlayerController.instance.IsAttacking) {
            var hitbox = other.GetComponent<IDamageable>();
            hitbox?.Damage(damage);
        }
    }
}
