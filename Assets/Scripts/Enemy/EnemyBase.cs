// Enemy AI Script
// Script that controls all enemies with 1 attack
// and basic functionality

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(BoxCollider))]
public class EnemyBase : MonoBehaviour, IDamageable
{
    EnemyBaseState _currentState;
    EnemyStateFactory _states;

    Animator _animator;
    NavMeshAgent _agent;

    [SerializeField] private int _health = 1;
    private bool _dead = false;

    [SerializeField] private int _damage = 10;
    private bool _isAttacking = false;

    // Detection
    Transform _target;
    [SerializeField] private float _sightRange = 2;
    [SerializeField] private float _patrolRadius = 2;
    [SerializeField] private float _circleRadius = 3.5f;
    [SerializeField] private float _attackRadius = 2;
    [SerializeField] private LayerMask _layerMask;

    bool _isWalking = false;

    static Collider[] _targetsBuffer = new Collider[100];

    // Animation States
    private int _walkHash;
    private int _attackHash;
    private int _hurtHash;
    private int _idleHash;
    private int _deathHash;


    #region getters and setters
    public EnemyBaseState CurrentState { get { return _currentState; } set { _currentState = value; }}

    public Animator Animator { get { return _animator; }}
    public NavMeshAgent Agent { get { return _agent; }}

    public Transform Target { get { return _target; }}
    public LayerMask LayerMask { get { return _layerMask; }}

    public bool Dead { get { return _dead; } set { _dead = value; }}
    public bool IsAttacking { get { return _isAttacking; } set { _isAttacking = value; }}

    public int Health { get { return _health; } set { _health = value; }}

    public int AttackHash { get { return _attackHash; }}
    public int WalkHash { get { return _walkHash; }}
    public int HurtHash { get { return _hurtHash; }}
    public int IdleHash { get { return _idleHash; }}
    public int DeathHash { get { return _deathHash; }}

    public float PatrolRadius { get { return _patrolRadius; }}
    public float AttackRadius { get { return _attackRadius; }}
    public float CircleRadius { get { return _circleRadius; }}
    #endregion


    public virtual void Awake() 
    {
        _states = new EnemyStateFactory(this);

        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        _states.GenerateStates();
        PrepValues();

        _states.SwitchState(EnemyStates.Patrol);
    }

    public virtual void Update() {
        _currentState.UpdateState();
    }

    public virtual void Damage(int damage) {
        if (!_dead) {
            Health -= damage;
            HurtEffect();

            if (Health <= 0) {
                _states.SwitchState(EnemyStates.Dead);
            }
        }
    }

    public virtual void PrepValues() 
    {
        GetComponentInChildren<EnemyWeapon>().SetDamage(_damage);

        _attackHash = Animator.StringToHash("Attack");
        _walkHash = Animator.StringToHash("Walk");
        _hurtHash = Animator.StringToHash("Hurt");
        _idleHash = Animator.StringToHash("Idle");
        _deathHash = Animator.StringToHash("Death");
    }

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
    
    public virtual void HurtEffect() {}

    public virtual void AdjustForWalking() {
        if (!_isWalking && _agent.velocity.magnitude > 1.7f) {
            _isWalking = true;
            _animator.Play(_walkHash);
        }
        else {
            _isWalking = false;
            _animator.Play(_idleHash);
        }
    }
    
    public virtual void TargetFound() {
        if (_target == null) {
            Debug.LogWarning("Incorrect function call!");
            return;
        }

        _states.SwitchState(EnemyStates.Combat);
    }

    public virtual void AttackComplete() { 
        _states.SwitchState(EnemyStates.Combat); 
    }

    public virtual void ReadyToAttack() {
        Vector3 direction = transform.position - _target.position;

        if (direction.sqrMagnitude < _attackRadius * _attackRadius) {
            LaunchAttack();
        } else {
            _states.SwitchState(EnemyStates.Combat);
        }
    }

    public virtual void LaunchAttack() {
        // Signposting effect
        _states.SwitchState(EnemyStates.Attack);
    }
}
