using UnityEngine;
using System.Collections;

public class EnemyIdleState : EnemyBaseState {
    public EnemyIdleState (EnemyStateMachine context, EnemyFactory factory) : base (context, factory) {}
    public override void Enter() {
        Ctx.Animator.SetBool(Ctx.IdleHash, true);

        Ctx.CurrentStateRoutine = Ctx.StartCoroutine(CheckPlayerDistance());
    }
    public override void UpdateState() {

    }
    public override void Exit() {
        Ctx.Animator.SetBool(Ctx.IdleHash, false);
    }

    IEnumerator CheckPlayerDistance() {
        while (true) {
            if (Ctx.SqrDistanceToTarget < Mathf.Pow(Ctx.DetectionRadius,2)) {
                Debug.Log("Player detected!");
                Factory.SwitchState(State.Stalk);
                break;
            } 
            
            yield return Ctx.ShortCheckDelay;
        }
    }
}