// Governs animation-based melee attacks on the enemy

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMelee : Attack
{
    public AttackMelee(EnemyBase context, int animationHash) : base (context, animationHash) {}

    public override void LaunchAttack() {
        float distance = (Ctx.Target.position - Ctx.transform.position).sqrMagnitude;
        if (distance < (Ctx.AttackRadius.x * Ctx.AttackRadius.x)) {
            Ctx.Animator.Play(AnimationHash);
        }
        else {
            Ctx.StartCoroutine(FinishAttack());
        }
    }

    IEnumerator FinishAttack() {
        Ctx.Agent.SetDestination(Ctx.Target.position);
        yield return new WaitForSeconds(
            Vector3.Distance(Ctx.Target.position, Ctx.transform.position)/
            Ctx.Agent.speed);
        if (!Ctx.Dead) {
            Ctx.Agent.SetDestination(Ctx.transform.position);
            Ctx.Animator.Play(AnimationHash);
        }
    }
}
