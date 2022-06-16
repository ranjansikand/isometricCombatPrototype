using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack
{
    EnemyBase _ctx;
    int _animationHash;

    public EnemyBase Ctx { get { return _ctx; }}
    public int AnimationHash { get { return _animationHash; }}

    public Attack (EnemyBase context, int animationHash) {
        _ctx = context;
        _animationHash = animationHash;
    }

    public abstract void LaunchAttack();
}
