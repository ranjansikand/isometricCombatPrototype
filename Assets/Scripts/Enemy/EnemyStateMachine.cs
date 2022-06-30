using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour, IDamageable
{
    // State-Machine Classes
    EnemyFactory _factory;
    EnemyBaseState _currentState;

    // Component Dependencies
    Animator _animator;
    NavMeshAgent _agent;

    // (Optional) Enemy Effects
    [SerializeField] EnemyHPBar _healthbar;

    // State-Machine controlled behaviors 
    private int _currentHealth;
    private bool _canHurt = true;

    Transform _target;
    Coroutine _currentStateRoutine;
    WaitForSeconds _shortCheckDelay = new WaitForSeconds(0.25f);

    // Animation behaviors
    int _attackHash,  // Trigger
        _hurtHash,    // Trigger
        _deadHash,    // Play State Directly
        _idleHash,    // Bool
        _runHash;     // Bool

    [SerializeField] EnemyData _data;

    public EnemyBaseState CurrentState { get { return _currentState; } set { _currentState = value; }}
    public Animator Animator { get { return _animator; }}
    public NavMeshAgent Agent { get { return _agent; }}
    public Transform Target { get { return _target; }}
    public Coroutine CurrentStateRoutine { get { return _currentStateRoutine; } 
        set { _currentStateRoutine = value; }}
    public WaitForSeconds ShortCheckDelay { get { return _shortCheckDelay; }}

    // Calculations
    public float SqrDistanceToTarget { get { return ((_target.position - transform.position).sqrMagnitude); }}
    public float CurrentAngle { get { return Vector3.Dot(
        (_target.position - transform.position).normalized, 
        transform.forward); }}
    public bool IsAttacking { get { return _animator.GetBool(_attackHash); }}

    // Animation Hashes
    public int AttackHash { get { return _attackHash; }}
    public int IdleHash { get { return _idleHash; }}
    public int HurtHash { get { return _hurtHash; }}
    public int DeadHash { get { return _deadHash; }}
    public int RunHash { get { return _runHash; }}

    // See EnemyData.cs for explanations of variable roles
    public float DetectionRadius { get { return _data.detectionRadius; }}
    public float AttackRange { get { return _data.attackRange; }}
    public float FollowRange { get { return _data.followRange; }}
    public float MaxAngle { get { return _data.maxAngle; }}
    public float TurnSpeed { get { return _data.turnSpeed; }}
    public float WalkSpeed { get { return _data.walkSpeed; }}
    public float RunSpeed { get { return _data.runSpeed; }}
    public Vector2 AttackDelay { get { return _data.attackDelay; }}


    // Automatically called functions
    void Awake() {
        // Load State Functionality
        _factory = new EnemyFactory(this);
        _factory.GenerateStates();

        // Access Components
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        // Translate Animation Strings
        _attackHash = Animator.StringToHash("Attack");
        _hurtHash = Animator.StringToHash("Hurt");
        _deadHash = Animator.StringToHash("Dead");
        _idleHash = Animator.StringToHash("Idle");
        _runHash = Animator.StringToHash("Run");

        // Initialize Behavior
        _currentHealth = _data.maxHealth;
        _healthbar?.InitializeBar(_currentHealth);
    }

    void Start() {
        _target = EnemyManager.instance.GetPoint();
        GetComponentInChildren<Weapon>().SetDamage(_data.damage);

        // Begin Behavior
        _factory.SwitchState(State.Idle);
    }

    void Update() {
        _currentState.UpdateState();
    }

    // Public functions
    public void TurnToPlayer() {
        var targetRotation = Quaternion.LookRotation (_target.position - transform.position);
        var str = Mathf.Min (_data.turnSpeed * Time.deltaTime, 1);
        transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, str);
    }

    public void DestroyThis() {
        Destroy(gameObject);
    }

    public void Damage(int damage) {
        if (_canHurt) {
            _currentHealth -= damage;

            if (_currentHealth > 0) Hurt();
            else Dead();
        }
    }

    // Private functions
    private void Hurt() {
        _canHurt = false;

        _healthbar?.UpdateBar(_currentHealth);
        _animator.SetTrigger(_hurtHash);

        Invoke(nameof(PostHurtReset), 0.25f);
    }
    private void PostHurtReset() { _canHurt = true; }

    private void Dead() {
        _canHurt = false;
        StopAllCoroutines();
        _factory.SwitchState(State.Dead);
        _healthbar?.UpdateBar(0);
    }

    
}
