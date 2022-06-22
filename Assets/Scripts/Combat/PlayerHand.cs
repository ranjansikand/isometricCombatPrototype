// Script attached to player's hand to allow punching damage

using UnityEngine;

public class PlayerHand : Weapon
{
    public override void OnTriggerEnter(Collider other) {
        if (PlayerController.instance.MainWeapon == null) {
            base.SetDamage(1 + PlayerController.instance._baseDamage);
            base.OnTriggerEnter(other);
        }
    }
}
