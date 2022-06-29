
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Combat
{
    private EnemyBase _ctx;
    private Vector3 _targetPos;
    private static WaitForSeconds _delay = new WaitForSeconds(0.5f);
    
    // private CombatAction _action;
    // private static int groundLayer = 1 << 3;

    public EnemyBase Ctx { get { return _ctx; }}

    // Class Constructor
    public Combat (EnemyBase context) {
        _ctx = context;
    }
    
    // Called on state enter
    public virtual void CombatBehavior() {
        Ctx.StartCoroutine(CheckPosition());
    }

    // EnemyBase.Update() > EnemyCombatStates.UpdateState() > Combat.CombatUpdate
    public virtual void CombatUpdate() {
        // Point towards player
        var targetRotation = Quaternion.LookRotation (Ctx.Target.position - Ctx.transform.position);
        var str = Mathf.Min (0.75f * Time.deltaTime, 1);
        Ctx.transform.rotation = Quaternion.Lerp (Ctx.transform.rotation, targetRotation, str);
    }

    IEnumerator CheckPosition() {
        while (!Ctx.IsAttacking) {
            float sqrMag = (Ctx.transform.position - Ctx.Target.position).sqrMagnitude;
            float sqrRad = Ctx.CircleRadius * Ctx.CircleRadius;
            // Check if player is far enough away to reposition
            if  ( sqrMag > 1.5f * sqrRad) {
                    // Head to new generated position
                    _targetPos = EnemyManager.instance.GetPoint().position;
                    Ctx.Agent.SetDestination(_targetPos);
            }
            // Go back to previous position after attacking
            else if (Ctx.JustAttacked) {
                Ctx.Agent.SetDestination(_targetPos);
            }

            yield return _delay;
        }
    }
}
