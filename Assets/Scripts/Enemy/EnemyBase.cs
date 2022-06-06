using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour, IDamageable
{
    EnemyBaseState _currentState;
    EnemyStateFactory _states;

    Animator _animator;
    NavMeshAgent _agent;

    [SerializeField] private int _health = 1;

    // Detection
    Transform _target;
    [SerializeField] private float _sightRange = 2, _patrolRadius = 2;
    [SerializeField] private LayerMask _layerMask;

    static Collider[] _targetsBuffer = new Collider[100];


    #region getters and setters
    public EnemyBaseState CurrentState { get { return _currentState; } set { _currentState = value; }}

    public Animator Animator { get { return _animator; }}
    public NavMeshAgent Agent { get { return _agent; }}

    public Transform Target { get { return _target; }}
    public LayerMask LayerMask { get { return _layerMask; }}

    public int Health { get { return _health; } set { _health = value; }}

    public float PatrolRadius { get { return _patrolRadius; }}
    #endregion


    public virtual void Awake() 
    {
        _states = new EnemyStateFactory(this);

        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        _states.GenerateStates();
        _states.SwitchState(EnemyStates.Patrol);
    }

    public virtual void Damage(int damage) {
        if (Health <= 0) return;

        Health -= damage;
        HurtEffect();

        if (Health <= 0) {
            _states.SwitchState(EnemyStates.Dead);
        }
    }

    public virtual void HurtEffect() {}

    public bool AcquireTarget () {
        Vector3 a = transform.localPosition;
        Vector3 b = a;
        b.y += 2f;
        int hits = Physics.OverlapCapsuleNonAlloc(
            a, b, _sightRange, _targetsBuffer, _layerMask);
        if (hits > 0) {
            for (int i = 0; i < hits; i++)
            {
                _target = _targetsBuffer[i].GetComponent<Collider>().gameObject.transform;
            }
            Debug.Assert(_target != null, "Targeted non-enemy!", _targetsBuffer[0]);
            return true;
        }
        _target = null;
        return false;
    }

    public virtual void TargetFound() {
        if (_target == null) {
            Debug.LogWarning("Incorrect function call!");
            return;
        }

        _states.SwitchState(EnemyStates.Combat);
    }
}
