// Script attached to player's hand to allow punching damage

using UnityEngine;

public class PlayerHand : Weapon
{
    void Start()
    {
        base.SetDamage(1);
    }

    public override void OnTriggerEnter(Collider other) {
        if (PlayerController.instance.MainWeapon == null) {
            base.OnTriggerEnter(other);
        }
    }
}
