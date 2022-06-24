using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject _slotPrefab;

    public List<InventorySlot> _slots = new List<InventorySlot>();

    Item _selectedItem;

    int _selectedSlot = 0;
    int _items = 0;

    public Item Selected { get { return _selectedItem; }}

    public void ShowMenu() {
        Debug.Log("1");
        // Add slots if insufficient in number
        int diff = PlayerInventory.instance.MaxSize - _slots.Count;
        if (diff > 0) {
            Debug.Log("2");
            for (int i = 0; i < diff; i++) {
                InventorySlot newSlot = Instantiate(_slotPrefab, transform).GetComponent<InventorySlot>();
                _slots.Add(newSlot);
                newSlot.ClearSlot();
            }
        }

        _items = 0;
        for (int j = 0; j < _slots.Count; j++) {
            Item item = PlayerInventory.instance.GetItemInSlot(j);
            if (item != null) {
                Debug.Log("3");
                _slots[j].UpdateSlot(item);
                _items++;
            }
        }        
    }

    public void HideMenu() {
        for (int j = 0; j < _slots.Count; j++) {
            _slots[j].ClearSlot();
        } 
    }

    private bool Select(int index) {
        if (_slots[index].HasItem) {
            _selectedItem = _slots[index].Select();
            _selectedSlot = index;
            return true;
        }
        return false;
    }

    public void ChangeSelectedItem(int direction) {
        if (Select(_selectedSlot + 1)) {
            return;
        }
        else if (direction < 0) {
            Select(_items - 1);
        }
        else {
            Select(0);
        }
    }

    public void UpdateSlots(Item item) {
        for (int j = 0; j < _slots.Count; j++) {
            if (_slots[j].Item == item) {
                _slots[j].ClearSlot();
                break;
            }
        }    
    }
}
