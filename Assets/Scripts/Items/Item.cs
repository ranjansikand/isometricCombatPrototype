// Base object for Inventory items
// Weapons and Talismans derive from this ScriptableObject

using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string _itemName;
    public string _description;
    
    public string ItemName { get { return _itemName; }}
    public string ItemDescription { get { return _description; }}
    public virtual bool IsEquippable { get { return false; }}
}
