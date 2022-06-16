using UnityEngine;
using UnityEngine.AI;

public abstract class Patrol
{
    // Private variables
    private static EnemyBase _context;

    private static int _groundLayer = 1 << 3;
    private Vector3 _startingPosition;

    private static WaitForSeconds _delay = new WaitForSeconds(0.5f);

    // Accessors to get and set private variables

    public EnemyBase Ctx { get { return _context; }}
    public NavMeshAgent Agent { get { return _context.Agent; }}

    public int GroundLayer { get { return _groundLayer; }}
    public WaitForSeconds Delay { get { return _delay; }}
    public Vector3 StartPos { get { return _startingPosition; } set { _startingPosition = value; }}

    // Class Constructor
    public Patrol(EnemyBase context) {
        _context = context;

        _startingPosition = context.transform.position;
    }

    public abstract void PatrolBehavior();
}
