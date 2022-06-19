using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class SimpleEnemy : MonoBehaviour, IDamageable {
    private Animator _animator;
    private NavMeshAgent _agent;

    public List<Transform> _potentialTargets = new List<Transform>();
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

    private void Awake() {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        _attackHash = Animator.StringToHash("Attack");
        _hurtHash = Animator.StringToHash("Hurt");
        _deathHash = Animator.StringToHash("Dead");

        _target = _potentialTargets[Random.Range(0, _potentialTargets.Count)];
        StartCoroutine(TargetNotFound());
    } 

    public void Damage(int amount) {
        if (!_dead && _health >= 0) {
            _health -= amount;
            
            if (_health <= 0) {
                Dead();
            }
        }
    }

    void Dead() {
        _dead = true;
        _agent.enabled = false;

        _animator.Play(_deathHash);
        ItemGenerator.instance.SpawnGold(transform.position, Random.Range(1, 10));

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
        if (!_dead && _targetInRange) {
            Vector3 direction = (_target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = lookRotation;
        }
    }
}