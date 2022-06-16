using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack1State : EnemyBaseState
{
    public EnemyAttack1State(EnemyBase currentContext) : base (currentContext) {}

    public override void EnterState() {
        Ctx.IsAttacking = true;
        Ctx.Attack1.LaunchAttack();
    }

    public override void UpdateState() {}

    public override void ExitState() {
        Ctx.IsAttacking = false;
        Ctx.JustAttacked = true;
    }
}
