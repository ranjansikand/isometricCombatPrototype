using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour, IDamageable
{
    [SerializeField] Item sword;

    void Start() {
        ItemGenerator.instance.SpawnObject(transform.position, sword);
    }

    public void Damage(int damage) {
        Debug.Log("Hit for " + damage + " damage");
    }
}
