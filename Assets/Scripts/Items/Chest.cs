// Treasure chest that spawns objects when interacted with
// Calls ItemGenerator to spawn a Loot object

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(BoxCollider), typeof(Rigidbody))]
public class Chest : MonoBehaviour, IDamageable
{
    private Animator animator;
    private bool open = false, opening = false;

    [SerializeField] bool locked = false;
    [SerializeField] ParticleSystem lockedEffect;

    void Start() {
        animator = GetComponent<Animator>();
    }

    public void Damage(int damage) {
        if (open) { return; } 
        else if (!opening && locked && !PlayerController.instance.HasKey()) {
            Locked();
            return; 
        }
        open = true;
        Open();
    }

    void Open() {
        // Play Animation and Effect
        animator.SetBool("Open", true);

        // Spawn Object
        GameObject buffer = ItemGenerator.instance.SpawnObject(transform.position);
            buffer.GetComponent<Rigidbody>().AddForce(
                3 * (transform.forward + Vector3.up), 
                ForceMode.Impulse);
    }

    void Locked() {
        lockedEffect.Play();
        Debug.Log("Locked");
    }
}
