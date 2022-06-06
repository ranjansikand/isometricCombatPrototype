using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerController currentContext) 
            : base (currentContext) {}

    public override void EnterState() {
        Ctx.NeedToSwitchToIdle = false;
        
        Ctx.Animator.Play(Ctx.StandardIdleHash);
    }
    public override void UpdateState() {}
    public override void ExitState() {}
}
