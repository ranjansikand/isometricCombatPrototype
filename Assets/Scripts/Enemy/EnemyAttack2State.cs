using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack2State : EnemyBaseState
{
    public EnemyAttack2State(EnemyBase currentContext) : base (currentContext) {}

    public override void EnterState() {
        Ctx.IsAttacking = true;
        Ctx.Attack2.LaunchAttack();
    }

    public override void UpdateState() {}

    public override void ExitState() {
        Ctx.IsAttacking = false;
        Ctx.JustAttacked = true;
    }
}
