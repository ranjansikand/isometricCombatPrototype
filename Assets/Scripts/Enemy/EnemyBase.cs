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

    [SerializeField]
    EnemyData _data;

    private int _health;

    private bool _dead = false;
    private bool _isAttacking = false;
    private bool _justAttacked = false;

    private Transform _target;
    private static Collider[] _targetsBuffer = new Collider[100];

    // Animation States
    private int _walkHash;
    private int _closeAttackHash;
    private int _rangeAttackHash;
    private int _hurtHash;
    private int _idleHash;
    private int _deathHash;

    // AI Modules
    private Patrol _patrol;
    private Combat _combat;
    private Attack _attack1;
    private Attack _attack2;


    #region getters and setters
    public EnemyBaseState CurrentState { get { return _currentState; } set { _currentState = value; }}

    public Animator Animator { get { return _animator; }}
    public NavMeshAgent Agent { get { return _agent; }}

    public Transform Target { get { return _target; }}

    public bool Dead { get { return _dead; } set { _dead = value; }}
    public bool IsAttacking { get { return _isAttacking; } set { _isAttacking = value; }}
    public bool JustAttacked { get { return _justAttacked; } set { _justAttacked = value; }}

    public int Health { get { return _health; } set { _health = value; }}

    public int IdleHash { get { return _idleHash; }}
    public int DeathHash { get { return _deathHash; }}

    public Vector2 AttackRadius { get { return _data.attackRadius; }}
    public float CircleRadius { get { return _data.circleRadius; }}

    public Patrol Patrol { get { return _patrol; }}
    public Combat Combat { get { return _combat; }}
    public Attack Attack1 { get { return _attack1; }}
    public Attack Attack2 { get { return _attack2; }}
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
        AdjustForWalking();
    }


    public virtual void PrepValues() {
        // Pull data from Scriptable Object
        _health = _data.maxHealth;
        GetComponentInChildren<EnemyWeapon>().SetDamage(_data.baseDamage);

        _closeAttackHash = Animator.StringToHash("Close Attack");
        _rangeAttackHash = Animator.StringToHash("Range Attack");
        _walkHash = Animator.StringToHash("walk");
        _hurtHash = Animator.StringToHash("Hurt");
        _idleHash = Animator.StringToHash("Idle");
        _deathHash = Animator.StringToHash("Dead");

        // set modules
        _patrol = _data.GeneratePatrolAction(this);
        _combat = new Combat(this);
        _attack1 = _data.GenerateAttack1(this, _closeAttackHash, _data.attackRole1);
        _attack2 = _data.GenerateAttack1(this, _closeAttackHash, _data.attackRole2);
    }


    public bool AcquireTarget () {
        Vector3 a = transform.localPosition;
        Vector3 b = a;
        b.y += 2f;
        int hits = Physics.OverlapCapsuleNonAlloc(
            a, b, _data.sightRange, _targetsBuffer, _data.layerMask);
        if (hits > 0) {
            for (int i = 0; i < hits; i++) {
                _target = _targetsBuffer[i].GetComponent<Collider>().gameObject.transform;
            }
            Debug.Assert(_target != null, "Targeted non-enemy!", _targetsBuffer[0]);
            return true;
        }
        _target = null;
        return false;
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

    public virtual void HurtEffect() {}

    public virtual void AdjustForWalking() {
        if (!_animator.GetBool(_walkHash) && _agent.velocity.sqrMagnitude > 1f) 
            _animator.SetBool(_walkHash, true); 
        if (_animator.GetBool(_walkHash) && _agent.velocity.sqrMagnitude < 1f)
            _animator.SetBool(_walkHash, false);
    }
    
    public virtual void TargetFound() {
        if (_target == null) { Debug.LogWarning("Target Error!"); return; }
        EnemyManager.instance.AddToEnemyList(this);
        _states.SwitchState(EnemyStates.Combat);
    }

    public virtual void AttackComplete() { 
        // Called by animation events to end animation
        _states.SwitchState(EnemyStates.Combat); 
    }

    public virtual void LaunchAttack() {
        _states.SwitchState(EnemyStates.CloseAttack);
    }
}
