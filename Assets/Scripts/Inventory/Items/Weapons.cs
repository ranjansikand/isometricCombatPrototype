using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Items/Weapon", order = 0)]
public class Weapons : Item
{
    public int _damage = 5;
    [Range(0f, 4f)] public float _knockback = 2f;
    public AnimationClip _attack1, _attack2;
    public AnimationClip _finisher;
    public AnimationClip _idle, _run;

    public GameObject _weapon;

    public override bool IsEquippable() { return true; }
}
