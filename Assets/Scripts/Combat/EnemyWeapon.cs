// Script for enemy weapons to damage player or other enemies

using UnityEngine;

public class EnemyWeapon : SimpleWeapon
{
    EnemyBase _activeState;

    void Awake() {
        _activeState = topParent.GetComponent<EnemyBase>();
    }
    
    public override void OnTriggerEnter(Collider other) {
        if (_activeState.IsAttacking && other.gameObject != topParent) {
            base.OnTriggerEnter(other);
        }
    }
      
}
