using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStates { Patrol, CloseAttack, RangeAttack, Combat, Dead }

public class EnemyStateFactory
{
    EnemyBase _context;

    // states
    private EnemyPatrolState _patrol;
    private EnemyAttack1State _attack1;
    private EnemyAttack2State _attack2;
    private EnemyCombatState _combat;
    private EnemyDeathState _death;

    public EnemyStateFactory(EnemyBase context) { 
        _context = context;
    }

    public void GenerateStates() 
    {
        _patrol = new EnemyPatrolState(_context);
        _attack1 = new EnemyAttack1State(_context);
        _attack2 = new EnemyAttack2State(_context);
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
            case (EnemyStates.CloseAttack): 
                newState = _attack1; break;
            case (EnemyStates.RangeAttack): 
                newState = _attack2; break;
            case (EnemyStates.Combat): 
                newState = _combat; break;
            case (EnemyStates.Dead): 
                newState = _death; break;
            default: 
                newState = _patrol; break;
        }

        _context.CurrentState = newState;
        _context.CurrentState.EnterState();
    }
}
