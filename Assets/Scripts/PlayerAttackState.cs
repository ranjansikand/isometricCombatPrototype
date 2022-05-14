using System.Collections;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    IEnumerator AttackResetRoutine() {
        yield return Ctx.ComboResetTimer;
        Ctx.AttackNumber = 1;
    }

    public PlayerAttackState(PlayerController currentContext) 
            : base (currentContext) {
        InitializeSubState();
    }
    public override void EnterState() {
        if (Ctx.ComboResetRoutine != null) Ctx.StopCoroutine(Ctx.ComboResetRoutine);

        if (Ctx.MainWeapon != null) {
            switch (Ctx.AttackNumber) {
                case (1): 
                    Debug.Log("Attack 1");
                    Ctx.Animator.Play(Ctx.MainWeapon._attack1.name);
                    Ctx.AttackNumber++;
                    break;
                case (2):
                    Debug.Log("Attack 2");
                    Ctx.Animator.Play(Ctx.MainWeapon._attack2.name);
                    Ctx.AttackNumber++;
                    break;
                case (3): 
                    Debug.Log("Finisher");
                    Ctx.Animator.Play(Ctx.MainWeapon._finisher.name);
                    Ctx.AttackNumber = 1;
                    break;
                default: 
                    Ctx.Animator.Play(Ctx.StandardAttackHash);
                    break;
            }
        }
        else {
            Ctx.Animator.Play(Ctx.StandardAttackHash);
        }
    }
    public override void UpdateState() {}
    public override void ExitState() {
        Ctx.ComboResetRoutine = Ctx.StartCoroutine(AttackResetRoutine());
    }
    public override void InitializeSubState() {}
}
