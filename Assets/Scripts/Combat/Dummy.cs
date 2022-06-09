using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour, IDamageable
{
    [SerializeField] int health = 1;

    Animator animator;
    bool dead = false;

    void Awake() {
        animator = GetComponent<Animator>();
    }

    public void Damage(int damage) {
        if (!dead) {
            health -= damage;

            if (health <= 0) 
            {
                dead = true;
                animator.Play("Death");
            }
        }
    }
}
