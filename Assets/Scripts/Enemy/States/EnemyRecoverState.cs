using System.Collections;
using UnityEngine;

public class EnemyRecoverState : EnemyBaseState {
    Vector3 _destination;
    public EnemyRecoverState (EnemyStateMachine context, EnemyFactory factory) 
        : base (context, factory) 
        {}
    public override void Enter() {
        var randomV2 = Random.insideUnitCircle * Ctx.FollowRange;
        _destination = Ctx.Target.position + new Vector3(randomV2.x, 0, randomV2.y);

        Ctx.Agent.SetDestination(_destination);
        Ctx.CurrentStateRoutine = Ctx.StartCoroutine(CheckPosition());
    }
    public override void UpdateState() {
        Ctx.TurnToPlayer();
    }
    public override void Exit() {
        
    }

    IEnumerator CheckPosition() {
        while (Vector3.Distance(Ctx.transform.position, _destination) > 1) {
            if (Ctx.SqrDistanceToTarget > Mathf.Pow(Ctx.DetectionRadius, 2)) {
                Ctx.StopAllCoroutines();
                Factory.SwitchState(State.Attack);
                break;
            }
            else {
                yield return Ctx.ShortCheckDelay;
            }
        }

        Factory.SwitchState(State.Idle);
    }
}