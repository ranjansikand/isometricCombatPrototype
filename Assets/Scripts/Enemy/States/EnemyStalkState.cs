using UnityEngine;
using System.Collections;

public class EnemyStalkState : EnemyBaseState {
    public EnemyStalkState (EnemyStateMachine context, EnemyFactory factory) : base (context, factory) {}
    public override void Enter() {
        Ctx.Agent.speed = Ctx.WalkSpeed;
        Ctx.StartCoroutine(PursueTime()); 
    }
    public override void UpdateState() {
        // Ctx.TurnToPlayer();

        var targetRotation = Quaternion.LookRotation (Ctx.Target.position - Ctx.transform.position);
        var str = Mathf.Min (Ctx.TurnSpeed * Time.deltaTime, 1);
        Ctx.transform.rotation = Quaternion.Lerp (Ctx.transform.rotation, targetRotation, str);
    }
    public override void Exit() {
        Ctx.StopAllCoroutines();
    }

    IEnumerator PursueTime() {
        var maxtime = Random.Range(Ctx.AttackDelay.x, Ctx.AttackDelay.y);
        float elapsedTime = 0;

        while (elapsedTime < maxtime && Ctx.SqrDistanceToTarget < Mathf.Pow(Ctx.DetectionRadius, 2)) {
            elapsedTime += Time.deltaTime;

            // Maintain follow range
            if (Ctx.SqrDistanceToTarget > Mathf.Pow(Ctx.FollowRange, 2)) {
                Ctx.Agent.SetDestination(Ctx.Target.position);
            } else {
                Ctx.Agent.SetDestination(Ctx.transform.position);
            }
            yield return null;
        }

        Factory.SwitchState(State.Attack);
    }
}