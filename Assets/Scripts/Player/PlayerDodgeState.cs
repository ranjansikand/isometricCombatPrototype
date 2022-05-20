using UnityEngine;

public class PlayerDodgeState : PlayerBaseState
{
    Vector3 _direction;
    float _dodgeSpeed;

    public PlayerDodgeState(PlayerController currentContext) 
            : base (currentContext) {
        InitializeSubState();
    }
    public override void EnterState() {
        // Lock direction to input at state entry
        _direction = Ctx.AppliedMovement != Vector3.zero ? 
            Ctx.AppliedMovement : 
            Vector3.forward;

        // Set dodge speed dependent on equipped talisman
        _dodgeSpeed = Ctx.EquippedTalisman != null ? 
            Ctx.DodgeSpeed * Ctx.EquippedTalisman._dodgeSpeedModifier : 
            Ctx.DodgeSpeed;

        Ctx.Animator.Play(Ctx.StandardDodgeHash);
    }
    public override void UpdateState() {
        Ctx.AppliedMovement = _direction * _dodgeSpeed;
        if (Ctx.CanMoveForward()) {
            Ctx.CharacterController.Move(Ctx.AppliedMovement * Time.deltaTime);
        } 
    }
    public override void ExitState() {}
    public override void InitializeSubState() {}
}
