using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "EnemyData/EnemyData", order = 0)]
public class EnemyData : ScriptableObject {
    public int maxHealth,
        damage;
    public float detectionRadius = 7.5f,  // Distance to first see player or run towards
        attackRange = 2.5f,               // Distance within which to launch attacks
        followRange = 5f;                 // Distance to maintain while recovering 
    public float maxAngle = 0.67f;        // Maximum side-angle for launching attacks
    public float turnSpeed = 0.5f,        // Rotation speed
        walkSpeed,                        // Normal movement speed (when player is within detectionRadius)
        runSpeed;                         // Speed when player is too far or before attack is launched
    public Vector2 attackDelay = Vector2.up;
}