using UnityEngine;

[CreateAssetMenu(fileName = "Talisman", menuName = "Items/Talisman", order = 1)]
public class Talismans : Item
{
    public float _walkSpeedModifer = 1.0f;
    public float _dodgeSpeedModifier = 1.0f;

    public string _statChangeDescription;

    public override bool IsEquippable { get { return true; }}
    public string StatChangeDescription { get { return _statChangeDescription; }}
}
