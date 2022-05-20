using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    float _speedSmoothVelocity;
    float _walkSpeed;

    public PlayerWalkState(PlayerController currentContext) 
            : base (currentContext) {
        InitializeSubState();
    }
    public override void EnterState() {
        Ctx.Animator.Play(Ctx.StandardRunHash);

        // Set walk speed dependent on equipped talisman
        _walkSpeed = Ctx.EquippedTalisman != null ? 
            Ctx.WalkSpeed * Ctx.EquippedTalisman._walkSpeedModifer : 
            Ctx.WalkSpeed;
    }
    public override void UpdateState() {
            HandleMotion();
            HandleRotation();
    }
    public override void ExitState() {
        Ctx.NeedToSwitchToIdle = false;
    }
    public override void InitializeSubState() {}

    void HandleMotion() {
        if (Ctx.CurrentMovementInput == Vector2.zero) {
            Ctx.NeedToSwitchToIdle = true;
            return;
        }
        Ctx.AppliedMovement = new Vector3(Ctx.CurrentMovementInput.x, 0, Ctx.CurrentMovementInput.y) * _walkSpeed;
        
        if (Ctx.CanMoveForward()) {
            Ctx.CharacterController.Move(Ctx.AppliedMovement * Time.deltaTime);
        }
    }

    void HandleRotation() {
        float targetRotation = Mathf.Atan2(Ctx.CurrentMovementInput.x, Ctx.CurrentMovementInput.y) * Mathf.Rad2Deg;
        Ctx.transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(Ctx.transform.eulerAngles.y, targetRotation, ref _speedSmoothVelocity, 0.1f);
    }
}
