// Base object for Inventory items
// Weapons and Talismans derive from this ScriptableObject

using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string _itemName;
    public string _description;
    public Sprite _icon;
    
    public string ItemName { get { return _itemName; }}
    public string ItemDescription { get { return _description; }}
    public Sprite Icon { get { return _icon; }}
    public virtual bool IsEquippable { get { return false; }}
}
