using UnityEngine;

public class DamageCalculator : MonoBehaviour {
    private static DamageCalculator instance;

    public DamageCalculator Instance { get { return instance; }}

    void Awake() {
        instance = this;
    }

    public int CalculateDamage(int damage, int defense) {
        return ((int) ((defense/100) * damage));
    }
}