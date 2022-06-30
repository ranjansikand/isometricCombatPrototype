// Potions that apply temporary buffs to player stats

using UnityEngine;

public enum PotionType {
    Speed = 1,
    Strength = 2
}

[CreateAssetMenu(fileName = "Potion", menuName = "Items/Stat Potion", order = 4)]
public class PotionsStats : Item {
    public PotionType adjustedStat;
}