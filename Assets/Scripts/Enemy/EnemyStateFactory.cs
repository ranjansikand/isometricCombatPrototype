using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStates { Patrol, Attack, Combat, Dead }

public class EnemyStateFactory : MonoBehaviour
{
    EnemyBase _context;

    // states
    EnemyPatrolState _patrol;
    EnemyAttackState _attack;
    EnemyCombatState _combat;
    EnemyDeathState _death;

    public EnemyStateFactory(EnemyBase context) { _context = context; }

    public void GenerateStates() 
    {
        _patrol = new EnemyPatrolState(_context);
        _attack = new EnemyAttackState(_context);
        _combat = new EnemyCombatState(_context);
        _death = new EnemyDeathState(_context);
    }

    public void SwitchState(EnemyStates desiredState) 
    {
        _context.CurrentState?.ExitState();
        EnemyBaseState newState;

        switch (desiredState) {
            case (EnemyStates.Patrol): 
                newState = _patrol; break;
            case (EnemyStates.Attack): 
                newState = _attack; break;
            case (EnemyStates.Combat): 
                newState = _combat; break;
            case (EnemyStates.Dead): 
                newState = _death; break;
            default: 
                newState = _patrol; break;
        }

        _context.CurrentState.EnterState();
    }
}
