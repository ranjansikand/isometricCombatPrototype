// Script for enemy weapons to damage player or other enemies

using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] GameObject topParent;
    EnemyBase _activeState;

    static int damage = 10;

    void Awake() {
        _activeState = topParent.GetComponent<EnemyBase>();
    }
    
    void OnTriggerEnter(Collider other) {
            if (_activeState.IsAttacking && other.gameObject != topParent) {
                var hitbox = other.GetComponent<IDamageable>();
                hitbox?.Damage(damage);
            }
        }
    
    public void SetDamage(int baseDamage) {
        damage = baseDamage;
    }    
}
