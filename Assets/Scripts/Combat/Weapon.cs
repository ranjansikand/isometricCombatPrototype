// Parent script for enemy hitboxes

using UnityEngine;

public class Weapon : MonoBehaviour {
    public int damage = 10;
    [SerializeField] public GameObject topParent;


    public virtual void OnTriggerEnter(Collider other) {
        var hitbox = other.GetComponent<IDamageable>();
        hitbox?.Damage(damage);
    }

    public virtual void SetDamage(int baseDamage) {
        damage = baseDamage;
    }  
}