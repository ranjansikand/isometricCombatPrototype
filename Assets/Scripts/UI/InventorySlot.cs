using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventorySlot : MonoBehaviour
{
    [SerializeField] Image iconSlot;
    [SerializeField] Image labelPanel;
    [SerializeField] Text label;
    [SerializeField] Button drop;

    private Item _item;

    public bool HasItem { get { return _item != null; }}
    public Item Item { get { return _item; }}

    public Item Select() {
        drop.gameObject.SetActive(true);
        // Add another option like use

        return _item;
    }

    public void UpdateSlot(Item item) {
        _item = item;
        
        iconSlot.gameObject.SetActive(true);
        labelPanel.gameObject.SetActive(true);

        iconSlot.sprite = item.Icon;
        label.text = item._itemName;
    }

    public void ClearSlot() {
        _item = null;
        
        iconSlot.sprite = null;
        label.text = "";

        iconSlot.gameObject.SetActive(false);
        labelPanel.gameObject.SetActive(false);
        drop.gameObject.SetActive(false);
    }
}
