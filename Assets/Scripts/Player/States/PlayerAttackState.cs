using System.Collections;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    IEnumerator AttackResetRoutine() {
        yield return Ctx.ComboResetTimer;
        Ctx.AttackNumber = 1;
    }

    public PlayerAttackState(PlayerController currentContext) 
            : base (currentContext) {}
            
    public override void EnterState() {
        if (Ctx.ComboResetRoutine != null) Ctx.StopCoroutine(Ctx.ComboResetRoutine);

        if (Ctx.MainWeapon != null) {
            switch (Ctx.AttackNumber) {
                case (1): 
                    Ctx.Animator.Play(Ctx.Attack1);
                    Ctx.AttackNumber++;
                    break;
                case (2):
                    Ctx.Animator.Play(Ctx.Attack2);
                    Ctx.AttackNumber++;
                    break;
                case (3): 
                    Ctx.Animator.Play(Ctx.Finisher);
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

    public override void UpdateState() {
        // slow to a stop
    }

    public override void ExitState() {
        Ctx.ComboResetRoutine = Ctx.StartCoroutine(AttackResetRoutine());
    }
}
