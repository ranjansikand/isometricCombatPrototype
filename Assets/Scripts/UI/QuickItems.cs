using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickItems : MonoBehaviour
{
    [SerializeField] ItemSlot[] _itemSlots;

    public void RedrawSlots(List<Item> inventory) {
        for (int i = 0; i < _itemSlots.Length; i++) {
            if (i > inventory.Count - 1) {
                _itemSlots[i].ClearItem();
            } else {
                _itemSlots[i].SetItem(inventory[i]);
            }
        }
    }
}
