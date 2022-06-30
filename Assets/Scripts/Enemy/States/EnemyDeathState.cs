using UnityEngine;
using System.Collections;

public class EnemyDeathState : EnemyBaseState {

    public EnemyDeathState (EnemyStateMachine context, EnemyFactory factory) : base (context, factory) {}
    public override void Enter() {
        Ctx.Agent.enabled = false;

        Ctx.Animator.Play(Ctx.DeadHash);

        Ctx.Invoke(nameof(Ctx.DestroyThis), 2.5f);
    }
    public override void UpdateState() {}
    public override void Exit() {}
}