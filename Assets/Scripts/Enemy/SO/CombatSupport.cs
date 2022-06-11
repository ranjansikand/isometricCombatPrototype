// Follow player at a set distance
// Behavior suits support troops i.e. healers

using System.Collections;
using UnityEngine;

public class CombatSupport : Combat
{
    WaitForSeconds _interval = new WaitForSeconds(0.5f);

    public CombatSupport (EnemyBase context) : base (context) {}

    public override void CombatBehavior() {
        base.CombatBehavior();
        Ctx.StartCoroutine(FollowPlayer());
    }

    IEnumerator FollowPlayer() {
        Ctx.Agent.SetDestination(Ctx.Target.position);

        while (true) {
            if (Ctx.Agent.remainingDistance > Ctx.CircleRadius) 
                Ctx.Agent.SetDestination(Target.position);
            else Ctx.Agent.SetDestination(Ctx.transform.position);

            yield return _interval;
        }
    }
}
