using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleWeapon : Weapon
{
    SimpleEnemy parentScript;

    void Awake() {
        parentScript = topParent.gameObject.GetComponent<SimpleEnemy>();
    }
    public override void OnTriggerEnter(Collider other)
    {
        if (parentScript.Attacking) base.OnTriggerEnter(other);
    }
}
