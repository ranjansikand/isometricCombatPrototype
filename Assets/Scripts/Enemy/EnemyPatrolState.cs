using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    private WaitForSeconds _delay = new WaitForSeconds(0.5f);

    public EnemyPatrolState(EnemyBase currentContext) : base (currentContext) {
        Ctx = currentContext;
    }

    public override void EnterState() {
        Ctx.StartCoroutine(CheckForTarget());
        Ctx.Patrol.PatrolBehavior();
    }

    public override void UpdateState() {}

    public override void ExitState() {
        Ctx.StopCoroutine(CheckForTarget());
    }

    IEnumerator CheckForTarget() {
        bool stillLooking = true;

        while (stillLooking) {
            if (Ctx.AcquireTarget()) {
                stillLooking = false;
                Ctx.TargetFound();
            }

            yield return _delay;
        }
    }
}
