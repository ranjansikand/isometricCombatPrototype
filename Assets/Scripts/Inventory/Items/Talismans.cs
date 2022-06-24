using UnityEngine;

[CreateAssetMenu(fileName = "Talisman", menuName = "Items/Talisman", order = 1)]
public class Talismans : Item
{
    public float _walkSpeedModifer = 1.0f;
    public float _dodgeSpeedModifier = 1.0f;
    public int _maxHealthChange = 0;
    public string _statChangeDescription;
    public override bool IsEquippable() { return true; }
}
