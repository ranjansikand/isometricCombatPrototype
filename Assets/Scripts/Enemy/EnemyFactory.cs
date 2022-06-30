using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State {
    Idle,
    Stalk,
    Attack,
    Recover,
    Dead
}

public class EnemyFactory
{
    EnemyStateMachine _ctx;

    // Standard States
    EnemyIdleState _idle;
    EnemyStalkState _stalk;
    EnemyAttackState _attack;
    EnemyRecoverState _recover;

    // Special States
    EnemyDeathState _dead;

    public EnemyFactory(EnemyStateMachine context) {
        _ctx = context;
    }

    public void GenerateStates() {
        // Standard
        _idle = new EnemyIdleState(_ctx, this);
        _stalk = new EnemyStalkState(_ctx, this);
        _attack = new EnemyAttackState(_ctx, this);
        _recover = new EnemyRecoverState(_ctx, this);

        // Special
        _dead = new EnemyDeathState(_ctx, this);
    }

    public EnemyBaseState GetState(State state) {
        switch (state) {
            // Standard
            case (State.Idle): return _idle;
            case (State.Stalk): return _stalk;
            case (State.Attack): return _attack;
            case (State.Recover): return _recover;

            // Special
            case (State.Dead): return _dead;
            default: return _idle;
        }
    }

    public void SwitchState(State state) {
        var newState = GetState(state);
        
        _ctx.CurrentState?.Exit();
        _ctx.CurrentState = newState;
        _ctx.CurrentState.Enter();
    }
}
