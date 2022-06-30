using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState
{
    EnemyStateMachine _ctx;
    EnemyFactory _factory;

    public EnemyStateMachine Ctx { get { return _ctx; }}
    public EnemyFactory Factory { get { return _factory; }}

    public EnemyBaseState(EnemyStateMachine context, EnemyFactory factory) {
        _ctx = context;
        _factory = factory;
    }

    public abstract void Enter();
    public abstract void UpdateState();
    public abstract void Exit();
}
