using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class SimpleEnemy : MonoBehaviour, IDamageable {
    private Animator _animator;
    private NavMeshAgent _agent;

    GameObject[] _potentialTargets;
    public static WaitForSeconds _delay = new WaitForSeconds(0.5f);

    bool _dead = false;
    bool _targetInRange = false;

    int _attackHash;
    int _deathHash;
    int _hurtHash;

    private Transform _target;
    [SerializeField] private int _health;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _sightRange;
    [SerializeField] EnemyHPBar _hpBar;

    public bool Attacking { get { return _animator.GetBool(_attackHash); }}
    public bool Dead { get { return _dead; }}

    private void Awake() {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        _attackHash = Animator.StringToHash("Attack");
        _hurtHash = Animator.StringToHash("Hurt");
        _deathHash = Animator.StringToHash("Dead");

        _potentialTargets = GameObject.FindGameObjectsWithTag("Target");
        _target = _potentialTargets[Random.Range(0, _potentialTargets.Length)].transform;
        StartCoroutine(TargetNotFound());

        _hpBar?.InitializeBar(_health);
    } 

    public void Damage(int amount) {
        if (!_dead && _health >= 0) {
            _health -= amount;
            
            _hpBar?.UpdateBar(_health);

            if (_health <= 0) {
                Die();
            }
        }
    }

    void Die() {
        _dead = true;
        _agent.enabled = false;

        _animator.Play(_deathHash);
        ItemGenerator.instance.PopOutGold(transform.position, Random.Range(1, 10));

        Invoke(nameof(DestroyThis), 2.5f);
    }

    void DestroyThis() {
        Destroy(gameObject);
    }

    IEnumerator TargetNotFound() {
        while (!_targetInRange) {
            if ((_target.position - transform.position).sqrMagnitude < (_sightRange * _sightRange)) {
                _targetInRange = true;
            }
            yield return _delay;
        }
        StartCoroutine(AttackingTarget());
    }

    IEnumerator AttackingTarget() {
        StopCoroutine(TargetNotFound());

        while (!_dead) {
            // Attack or move
            if ((_target.position - transform.position).sqrMagnitude < _attackRange) {
                _animator.SetBool(_attackHash, true);
            } else {
                _animator.SetBool(_attackHash, false);
                _agent.SetDestination(_target.position);
            }
            yield return _delay;
        }
    }

    void Update() {
        if (!_dead && _targetInRange && !_animator.GetBool(_attackHash)) {
            Vector3 direction = (_target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = lookRotation;
        }
    }
}