using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator), typeof(Rigidbody))]
[RequireComponent(typeof(Collider), typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour, IDamageable {
    // Components assigned in Awake
    private Animator _animator;
    private NavMeshAgent _agent;

    Transform _target;
    Coroutine _currentAction;
    WaitForSeconds _delay = new WaitForSeconds(0.5f), 
        _postAttackDelay;

    bool _dead = false;
    bool _hurt = false;
    bool _lookAtTarget = false;

    // Animation Hashes
    int _attackHash,  // Trigger
        _hurtHash,    // Trigger
        _deadHash,    // Play State Directly
        _idleHash;    // Bool

    // Customizable Variables
    [SerializeField] int health,
        damage;
    [SerializeField] float _detectionRadius = 7.5f,  // Distance to detect player
        _attackRange = 2.5f,      // Distance within which to launch attacks
        _followRange = 5f;      // Distance to maintain while recovering 
    [SerializeField] float _maxAngle = 0.67f;  // Maximum side-angle for launching attacks
    [SerializeField] float _turnSpeed = 0.5f;
    [SerializeField] float _attackDelay = 2f;

    // Enemy Effects
    [SerializeField] EnemyHPBar _hpBar;


    // Controls pseudo-state machine behaviour
    private void NextAction(IEnumerator action) {
        if (_currentAction != null) StopCoroutine(_currentAction);
        _currentAction = StartCoroutine(action);
    }


    #region General Functions
    private void Awake() {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        _attackHash = Animator.StringToHash("Attack");
        _hurtHash = Animator.StringToHash("Hurt");
        _deadHash = Animator.StringToHash("Dead");
        _idleHash = Animator.StringToHash("Idle");


        GetComponentInChildren<Weapon>().SetDamage(damage);
    }

    private void Start() {
        _target = EnemyManager.instance.GetPoint();
        _currentAction = StartCoroutine(CheckDistance());

        _hpBar?.InitializeBar(health);
        _postAttackDelay = new WaitForSeconds(_attackDelay);
    }

    public void Damage(int damage) {
        if (!_dead && !_hurt) {
            health -= damage;

            if (health <= 0) Dead();
            else Hurt();
        }
    }

    private void Hurt() {
        _animator.SetTrigger(_hurtHash);
        _hpBar?.UpdateBar(health);

        _hurt = true;
        StartCoroutine(Recover());
    }

    IEnumerator Recover() {
        yield return _delay;
        _hurt = false;
    }

    private void Dead() {
        _currentAction = null;
        StopAllCoroutines();

        _hpBar?.UpdateBar(0);
        _animator.Play(_deadHash);

        _dead = true;
        _lookAtTarget = false;
        _agent.enabled = false;

        ItemGenerator.instance.PopOutObject(transform.position);

        Invoke(nameof(DestroyThis), 2.5f);
    }

    void DestroyThis() {
        Destroy(gameObject);
    }

    private void Update() {
        if (_lookAtTarget && !_dead) {
            var targetRotation = Quaternion.LookRotation (_target.position - transform.position);
            var str = Mathf.Min (_turnSpeed * Time.deltaTime, 1);
            transform.rotation = Quaternion.Lerp (transform.rotation, targetRotation, str);
        }
    }
    #endregion

    #region Searching for the player
    IEnumerator CheckDistance() {
        _animator.SetBool(_idleHash, true);

        while ((_target.position - transform.position).sqrMagnitude > 
                (_detectionRadius * _detectionRadius)) {
            yield return _delay;
        }
        
        _animator.SetBool(_idleHash, false);
        NextAction(GetIntoAttackPosition());
    }
    #endregion

    #region Pre-Attack
    IEnumerator GetIntoAttackPosition() {
        _lookAtTarget = true;

        float dot = -1;
        while ((_target.position - transform.position).sqrMagnitude 
                > (_attackRange * _attackRange) && dot < _maxAngle) {
            var heading = (_target.position - transform.position).normalized;
            dot = Vector3.Dot(heading, transform.forward);

            _agent.SetDestination(_target.position);

            yield return _delay;
        }

        _agent.enabled = false;
        _lookAtTarget = false;
        Attack();

        NextAction(FallBackToFollowRange());
    }
    #endregion

    void Attack() {
        _animator.SetTrigger(_attackHash);
    }

    #region Post-Attack
    IEnumerator FallBackToFollowRange() {
        yield return _postAttackDelay;
        _lookAtTarget = true;
        _agent.enabled = true;

        bool ready = false;
        Vector3 desiredPos = GenerateNewPos();

        while (!ready) {
            if ((_target.position - transform.position).sqrMagnitude > 
                    (_detectionRadius * _detectionRadius)) {
                desiredPos = GenerateNewPos();
            }
            else if ((desiredPos - transform.position).sqrMagnitude < 
                    (_followRange * _followRange)) {
                ready = true;
            }

            _agent.SetDestination(desiredPos);
            yield return _delay;
        }

        NextAction(GetIntoAttackPosition());
    }

    Vector3 GenerateNewPos() {
        var randomVector2 = Random.insideUnitCircle * _followRange;
        return _target.position + new Vector3(randomVector2.x, 0, randomVector2.y);
    }
    #endregion
}