using UnityEngine;


public enum PatrolAction { None,Circle,Sleep }
public enum CombatAction { Melee, Range }
public enum AttackAction { Melee, Range }


[CreateAssetMenu(fileName = "EnemyData", menuName = "EnemyAI/EnemyData", order = 1)]
public class EnemyData : ScriptableObject 
{
    public int maxHealth = 1;
    public int baseDamage = 10;

    public float sightRange = 2;
    public float circleRadius = 3.5f;

    public Vector2 attackRadius = Vector2.one;
    public LayerMask layerMask;
    public PatrolAction patrolAction;
    public CombatAction combatRole;
    public AttackAction attackRole1;
    public AttackAction attackRole2;

    public Patrol GeneratePatrolAction(EnemyBase context) {
        switch (patrolAction) {
            case (PatrolAction.None): return new PatrolNone(context);
            case (PatrolAction.Circle): return new PatrolCircle(context);
            case (PatrolAction.Sleep): return new PatrolSleep(context);
            default: return new PatrolNone(context);
        }
    }

    public Attack GenerateAttack1(EnemyBase context, int animation, AttackAction attackRole) {
        switch (attackRole) {
            case (AttackAction.Melee): return new AttackMelee(context, animation);
            default: return new AttackMelee(context, animation);
        }
    }
}

