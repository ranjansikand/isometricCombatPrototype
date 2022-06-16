// Stay in spawn location and sleep

using System.Collections;
using UnityEngine;

public class PatrolSleep : Patrol
{
    public PatrolSleep(EnemyBase context) : base (context) {}

    public override void PatrolBehavior() {
        Ctx.Animator.Play("Sleep");
        Ctx.StartCoroutine(WakeUp());
    }   

    IEnumerator WakeUp() {
        while (Ctx.Target == null) {
            yield return Delay;
        }
        Ctx.Animator.Play(Ctx.IdleHash);
    }
}
