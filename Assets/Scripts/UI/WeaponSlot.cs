using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour
{
    private Item thisItem;
    [SerializeField] private Image icon;
    [SerializeField] private GameObject selection;

    public void Select() {
        selection.SetActive(true);
    }

    public void Deselect() {
        selection.SetActive(false);
    }

    public void SetItem(Item item) {
        thisItem = item;
        icon.sprite = item.Icon;
    }

    public void DropItem() {
        thisItem = null;
        icon.sprite = null;
    }
}
