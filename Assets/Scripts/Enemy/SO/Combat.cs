
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class Combat
{
    private EnemyBase _ctx;
    private Transform _target;

    public EnemyBase Ctx { get { return _ctx; }}
    public Transform Target { get { return _target; }}

    public Combat (EnemyBase context) {
        _ctx = context;
    }

    public virtual void CombatUpdate() {
        Vector3 direction = (_target.position - Ctx.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        Ctx.transform.rotation = lookRotation;
    }

    public virtual void CombatBehavior() {
        if (Ctx.Target == null) {
            Debug.Log("No target!");
        }
        else _target = Ctx.Target;
        Ctx.StartCoroutine(ReadyToAttack());
    }

    IEnumerator ReadyToAttack() {
        yield return new WaitForSeconds(Random.Range(1f, 3.5f));
        Ctx.ReadyToAttack();
    }
}
