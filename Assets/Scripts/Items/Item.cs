using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {
    Weapon, 
    Talisman
}

public abstract class Item : ScriptableObject
{
    public string _itemName;
}
