// Base object for Inventory items

using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string _itemName;

    public string ItemName { get { return _itemName; }}
    public virtual bool IsEquippable { get { return false; }}
}
