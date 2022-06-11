// Do not move or patrol

using UnityEngine;

public class PatrolNone : Patrol
{
    public PatrolNone(EnemyBase context) : base (context) {}
    public override void PatrolBehavior() {}
}
