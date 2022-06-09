using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatState : EnemyBaseState
{
    public EnemyCombatState(EnemyBase currentContext) : base (currentContext) {}

    public override void EnterState() {
        Ctx.StartCoroutine(DelayBetweenAttacks());
        Ctx.Agent.SetDestination(GeneratePosition());
    }

    public override void UpdateState() {
        Vector3 direction = (Ctx.Target.position - Ctx.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        Ctx.transform.rotation = lookRotation;
    }

    public override void ExitState() {
        Ctx.StopAllCoroutines();
    }


    IEnumerator DelayBetweenAttacks() {
        yield return new WaitForSeconds(Random.Range(1f, 3.5f));

        Ctx.ReadyToAttack();
    }

    Vector3 GeneratePosition() {
        float dest = Random.Range(0, 10);
        Vector2 buffer = Random.insideUnitCircle * ((dest > 3f)? Ctx.CircleRadius : Ctx.AttackRadius);
        
        return Ctx.Target.position + new Vector3(buffer.x, 0, buffer.y);
    }
}
