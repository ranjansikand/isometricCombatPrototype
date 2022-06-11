using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatState : EnemyBaseState
{
    public EnemyCombatState(EnemyBase currentContext) : base (currentContext) {}

    public override void EnterState() {
        Ctx.Combat.CombatBehavior();
    }

    public override void UpdateState() {
        Ctx.Combat.CombatUpdate();
    }

    public override void ExitState() {
        Ctx.StopAllCoroutines();
    }
}
