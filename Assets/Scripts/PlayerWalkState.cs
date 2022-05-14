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
        // Check if current weapon overrides run animation
        if (Ctx.MainWeapon?._run != null) {
            Ctx.Animator.Play(Ctx.MainWeapon._run.name);
        } else {
            Ctx.Animator.Play(Ctx.StandardRunHash);
        }

        // Set walk speed dependent on equipped talisman
        _walkSpeed = Ctx.EquippedTalisman != null ? 
            Ctx.WalkSpeed * Ctx.EquippedTalisman._walkSpeedModifer : 
            Ctx.WalkSpeed;
    }
    public override void UpdateState() {
        if (Ctx.CurrentMovementInput == Vector2.zero) {
            Ctx.NeedToSwitchToIdle = true;
            return;
        }
        Ctx.AppliedMovement = new Vector3(Ctx.CurrentMovementInput.x, 0, Ctx.CurrentMovementInput.y) * _walkSpeed;
        Ctx.CharacterController.Move(Ctx.AppliedMovement * Time.deltaTime);

        HandleRotation();
    }
    public override void ExitState() {
        Ctx.NeedToSwitchToIdle = false;
    }
    public override void InitializeSubState() {}

    void HandleRotation() {
        float targetRotation = Mathf.Atan2(Ctx.CurrentMovementInput.x, Ctx.CurrentMovementInput.y) * Mathf.Rad2Deg;
        Ctx.transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(Ctx.transform.eulerAngles.y, targetRotation, ref _speedSmoothVelocity, 0.1f);
    }
}
