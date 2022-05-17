// Script controlling individual panel slots for GUI

using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    // Visual Elements
    private Text _label;
    private GameObject _use, _drop;

    // Contained Data
    private Item _item;
    private int _index;
    private bool _assigned;

    public Item Item { get { return _item; } set { _item = value; }}


    void Awake() {
        // Check if components were assigned
        if (_label == null) {
            _label = GetComponentInChildren<Text>();
            _use = gameObject.transform.Find("Use").gameObject;
            _drop = gameObject.transform.Find("Remove").gameObject;
        }

        PrepObject();
    }

    public void PrepObject(Item item = null, int index = 21, bool status = false) {
        _item = item;
        _index = index;
        _assigned = status;

        _use.SetActive(_assigned);
        _drop.SetActive(_assigned);
        _label.gameObject.SetActive(_assigned);

        if (_assigned) {
            _label.text = _item.ItemName;
            if (_item.IsEquippable) {
                _use.GetComponentInChildren<Text>().text = "Equip";
            }
        }
    }

    public void Use() {
        if (_item.IsEquippable) {
            Inventory.instance.UseItem(_index);
        }
    }

    public void Drop()
    {
        if (_assigned) {
            ItemGenerator.instance.SpawnObject(
                PlayerController.instance.CurrentPosition + 
                PlayerController.instance.AppliedMovement * 2, 
                _item);
            Inventory.instance.Remove(_index);
        }

        _assigned = false;
        PrepObject();
    }
}
