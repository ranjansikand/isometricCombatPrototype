using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public delegate void InventoryChangedDelegate(Item item);
    public static InventoryChangedDelegate itemEquipped;

    private int _capacity = 20;
    private List<Item> _inventory = new List<Item>();

    public GameObject _panel;
    public List<InventorySlot> _slots = new List<InventorySlot>();

    void Awake() {
        if (instance != null) {
            Debug.LogWarning("Multiple inventories found!");
            return;
        }
        instance = this;

        if (_slots.Count < _capacity) {
            Debug.LogWarning("Insufficient inventory slots!");
        }

        PrepareInventory(false);
    }

    private void PrepareInventory(bool reveal)
    {
        _capacity = _capacity - _inventory.Count;

        for (int i = 0; i < Mathf.Min(_inventory.Count, _slots.Count); i++) {
            AssignSlot(_slots[i], i);
        }

        _panel.SetActive(reveal);
    }

    void AssignSlot(InventorySlot slot, int index) {
        slot.Item = _inventory[index];
        slot.Index = index;
        slot.PrepObject();
    }


    #region called by the Player Controller
    public bool AddItem (Item newItem) 
    {
        if (_capacity <= 0) {
            Debug.Log("Inventory full!");
            return false;
        }
        else {
            _inventory.Add(newItem);
            _capacity -= 1;
            Debug.Log("Acquired " + newItem._itemName+ ". " + _capacity + " spaces remaining.");

            return true; 
        }
    }

    public void ToggleInventory() {
        PrepareInventory(!_panel.activeSelf);
    }
    #endregion
}
