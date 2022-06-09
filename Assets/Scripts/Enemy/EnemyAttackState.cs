using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(EnemyBase currentContext) : base (currentContext) {}

    public override void EnterState() {
        Ctx.IsAttacking = true;

        Ctx.Animator.Play(Ctx.AttackHash);
    }

    public override void UpdateState() {}

    public override void ExitState() {
        Ctx.IsAttacking = false;
    }
}
