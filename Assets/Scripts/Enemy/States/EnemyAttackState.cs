using System.Collections;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState {
    bool validAngle;

    public EnemyAttackState (EnemyStateMachine context, EnemyFactory factory) : base (context, factory) {}
    public override void Enter() {
        bool withinAttackRange = Ctx.SqrDistanceToTarget < (Mathf.Pow(Ctx.AttackRange, 2));
        validAngle = Ctx.CurrentAngle <= Ctx.MaxAngle;
        
        if (withinAttackRange && validAngle) {
            Attack();
        } 
        else {
            Ctx.CurrentStateRoutine = Ctx.StartCoroutine(RushTarget());
        }
    }

    public override void UpdateState() {
        if (!validAngle) {
            Ctx.TurnToPlayer();
        }
    }

    public override void Exit() {
        Ctx.Agent.enabled = true;
        Ctx.Agent.speed = Ctx.WalkSpeed;
        Ctx.Animator.ResetTrigger(Ctx.HurtHash);
    }

    void Attack() {
        Ctx.Agent.SetDestination(Ctx.transform.position);
        Ctx.Agent.enabled = false;
        Ctx.Animator.SetBool(Ctx.AttackHash, true);

        Ctx.StopAllCoroutines();
        Ctx.CurrentStateRoutine = Ctx.StartCoroutine(CheckAttackCompleted());
    }

    IEnumerator RushTarget() {
        Ctx.Animator.SetBool(Ctx.RunHash, true);
        Vector3 destination = Ctx.Target.position;
        Ctx.Agent.speed = Ctx.RunSpeed;
        Ctx.Agent.SetDestination(destination); 

        while (Vector3.Distance(destination, Ctx.transform.position) > Ctx.AttackRange) {
            yield return Ctx.ShortCheckDelay;
        }

        Ctx.Animator.SetBool(Ctx.RunHash, false);
        Attack();
    }

    IEnumerator CheckAttackCompleted() {
        while (Ctx.Animator.GetBool(Ctx.AttackHash)) {
            yield return Ctx.ShortCheckDelay;
        }

        Factory.SwitchState(State.Recover);
    }
}