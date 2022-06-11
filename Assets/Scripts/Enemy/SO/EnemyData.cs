using UnityEngine;


public enum PatrolAction { None,Circle,Sleep }
public enum CombatAction { Melee, Range, Support }


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

    public Patrol GeneratePatrolAction(EnemyBase context) {
        switch (patrolAction) {
            case (PatrolAction.None): return new PatrolNone(context);
            case (PatrolAction.Circle): return new PatrolCircle(context);
            case (PatrolAction.Sleep): return new PatrolSleep(context);
            default: return new PatrolNone(context);
        }
    }

    public Combat GenerateCombatAction(EnemyBase context) {
        switch (combatRole) {
            // case (CombatAction.Melee): return new CombatMelee(context);
            case (CombatAction.Support): return new CombatSupport(context);
            // case (CombatAction.Range): return new CombatRange(context);
            default: return new CombatSupport(context);
        }
    }
}

