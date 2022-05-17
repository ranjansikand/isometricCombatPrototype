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

        PrepareInventory();
    }

    private void PrepareInventory(bool reveal = false)
    {
        _capacity = _capacity - _inventory.Count;

        for (int i = 0; i < Mathf.Min(_inventory.Count, _slots.Count); i++) {
            if (_slots[i] != null && _inventory[i] != null) {
                _slots[i].PrepObject(_inventory[i], i, true);
            }
        }
    
        _panel.SetActive(reveal);
    }

    #region public functions
    // Called by Player Controller on item pickup
    public bool AddItem (Item newItem) 
    {
        if (_capacity <= 0) {
            Debug.Log("Inventory full!");
            return false;
        }
        else {
            _inventory.Add(newItem);
            _capacity -= 1;
            Debug.Log("Acquired " + newItem.ItemName+ ". " + _capacity + " spaces remaining.");

            return true; 
        }
    }

    // Called by Player Controller via input event
    public void ToggleInventory() {
        PrepareInventory(!_panel.activeSelf);
    }

    // Called by Inventory Slot via button clicks
    public void UseItem(int index) {
        if (_slots[index].Item == _inventory[index]) {
            itemEquipped(_inventory[index]);
            _inventory.RemoveAt(index);
        } else {
            Debug.LogWarning("Inventory mismatch!");
            PrepareInventory(_panel.activeSelf);
        }
    }

    public void Remove(int index) {
        _inventory.RemoveAt(index);
        PrepareInventory();
    }
    #endregion
}
