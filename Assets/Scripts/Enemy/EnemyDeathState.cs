using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathState : EnemyBaseState
{
    public EnemyDeathState(EnemyBase currentContext) : base (currentContext) {}

    public override void EnterState() {
        Ctx.Dead = true;
        EnemyManager.instance.RemoveFromCombat(Ctx);

        Ctx.GetComponent<BoxCollider>().enabled = false;
        Ctx.Agent.enabled = false;

        Ctx.Animator.Play(Ctx.DeathHash);
        Ctx.enabled = false;
    }

    public override void UpdateState() {}

    public override void ExitState() {}
}
