
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Combat
{
    private EnemyBase _ctx;
    private CombatAction _action;

    private Vector3 _targetPos;
    private static int groundLayer = 1 << 3;
    private static WaitForSeconds _delay = new WaitForSeconds(0.5f);
    

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
        Vector3 direction = (Ctx.Target.position - Ctx.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        Ctx.transform.rotation = lookRotation;
    }

    IEnumerator CheckPosition() {
        while (!Ctx.IsAttacking) {
            float sqrMag = (Ctx.transform.position - Ctx.Target.position).sqrMagnitude;
            float sqrRad = Ctx.CircleRadius * Ctx.CircleRadius;
            // Check if player is too far or too close
            if  ( sqrMag > 1.5f * sqrRad || Ctx.JustAttacked) {
                    // Head to new generated position
                    Ctx.Agent.SetDestination(GeneratePosition());
            }

            yield return _delay;
        }
    }

    Vector3 GeneratePosition() {
        // Assign point guaranteed to fail raycast
        Vector3 checkPos = new Vector3(0,100,0);

        // Choose new point in a circle around the player
        while (!Physics.Raycast(checkPos, Vector3.down, 2f, groundLayer)) {
            // Generate random direction x random magnitude
            Vector2 temp = Random.insideUnitCircle.normalized * (Ctx.CircleRadius * Random.Range(0.8f, 1.2f));
            // Center to player's position
            checkPos = new Vector3(temp.x, 0, temp.y) + Ctx.Target.position;
            // Set y to original y
            checkPos.y = Ctx.transform.position.y;
        }

        if (Ctx.JustAttacked) Ctx.JustAttacked = false;
        return checkPos;
    }
}
