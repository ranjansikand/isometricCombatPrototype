using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerBaseState
{
    public PlayerDeathState(PlayerController currentContext) 
            : base (currentContext) {}

    public override void EnterState() {
        Ctx.Animator.Play("Death");
    }
    
    public override void UpdateState() {}
    public override void ExitState() {}
}
