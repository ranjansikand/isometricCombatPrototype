// Move within a predefined circle from spawn point

using System.Collections;
using UnityEngine;

public class PatrolCircle : Patrol
{
    private float _patrolRadius;

    public PatrolCircle(EnemyBase context) : base (context) {
        _patrolRadius = Random.Range(4f, 10f);
    }

    public override void PatrolBehavior() {
        Ctx.StartCoroutine(CircleArea());
    }

    IEnumerator CircleArea() {
        while (Ctx.Target == null) {
            if (Agent.remainingDistance < 1.0f) { 
                yield return new WaitForSeconds(Random.Range(0.5f, 2f));
                Agent.SetDestination(GeneratePosition()); 
            }
            yield return Delay;
        }

        Agent.SetDestination(Ctx.transform.position);
    }

    Vector3 GeneratePosition() {
        Vector3 checkPos = new Vector3(0, 100, 0);
        Vector2 buffer = Vector2.one;

        // Generate a new point up to 60 times
        for (int i = 0; i < 60; i++) {
            if (Physics.Raycast(checkPos, Vector3.down, 2f, GroundLayer)) {
                break;
            }
            else {
                buffer = Random.insideUnitCircle * _patrolRadius;
                checkPos = StartPos + new Vector3(buffer.x, 0, buffer.y);
            }
        }

        return checkPos;
    }
}
