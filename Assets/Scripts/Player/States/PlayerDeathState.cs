public class PlayerDeathState : PlayerBaseState
{
    public PlayerDeathState(PlayerController currentContext) 
            : base (currentContext) {}

    public override void EnterState() {
        Ctx.Animator.SetBool("Dead", true);
    }
    
    public override void UpdateState() {
        if (!Ctx.Animator.GetBool("Dead")) {
            Ctx.Animator.SetBool("Dead", true);
        }
    }
    public override void ExitState() {}
}
