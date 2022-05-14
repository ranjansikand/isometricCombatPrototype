using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerController currentContext) 
            : base (currentContext) {
        InitializeSubState();
    }
    public override void EnterState() {
        Ctx.NeedToSwitchToIdle = false;
        
        if (Ctx.MainWeapon?._idle != null) {
            Ctx.Animator.Play(Ctx.MainWeapon._idle.name);
        } else {
            Ctx.Animator.Play(Ctx.StandardIdleHash);
        }
    }
    public override void UpdateState() {}
    public override void ExitState() {}
    public override void InitializeSubState() {}
}
