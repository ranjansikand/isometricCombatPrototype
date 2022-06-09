using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    #region variables
    Vector3 _startingPosition;
    Vector3 _destination;
    int _groundLayer = 1 << 3;
    WaitForSeconds _delay = new WaitForSeconds(0.5f);
    #endregion

    public EnemyPatrolState(EnemyBase currentContext) : base (currentContext) {}

    public override void EnterState() {
        _startingPosition = Ctx.transform.position;
        Ctx.StartCoroutine(CheckForTarget());
    }

    public override void UpdateState() {}

    public override void ExitState() {}

    #region state-specific functions
    IEnumerator CheckForTarget() {
        bool stillLooking = true;

        while (stillLooking) {
            if (Ctx.AcquireTarget()) {
                stillLooking = false;
                Ctx.TargetFound();
            }

            HandleMotion();
            yield return _delay;
        }
    }

    void PickLocation() {
        Vector3 tempDest = _startingPosition + new Vector3(
            Random.Range(-Ctx.PatrolRadius, Ctx.PatrolRadius), 0,
            Random.Range(-Ctx.PatrolRadius, Ctx.PatrolRadius));

        if (Physics.Raycast(tempDest, -Ctx.transform.up, 2f, _groundLayer)) {
            _destination = tempDest;
        }
        else {
            PickLocation();
        }
    }

    bool Arrived() {
        if (_destination == null) return true;
        return Ctx.Agent.remainingDistance < 1.0f;
    }

    void HandleMotion() {
        if (Ctx.Target != null) {
            Ctx.Agent.ResetPath();
            return;
        }
        if (Arrived()) {
            PickLocation();
            Ctx.Agent.SetDestination(_destination);
        }
    }
    #endregion
}
