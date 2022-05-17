using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    // Visual Elements
    private Text _label;
    private Button _use, _drop;

    // Contained Data
    private Item _item;
    private int _index;
    private bool _assigned = false;

    public Item Item { get { return _item; } set { _item = value; _assigned = true; }}
    public int Index { get { return _index; } set { _index = value; _assigned = true; }}


    void Awake() {
        if (_label == null) _label = GetComponentInChildren<Text>();
    }

    public void PrepObject() {
        if (_assigned) {
            _label.text = _item?._itemName;
        } else {
            _label.text = "Shit burgular";
        }
    }

}
