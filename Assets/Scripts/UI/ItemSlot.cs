// Controls UI Elements for Quick Item

using UnityEngine;
using UnityEngine.UI;
public class ItemSlot : MonoBehaviour
{
    private Item thisItem;
    [SerializeField] private Image icon;

    public void SetItem(Item item) {
        thisItem = item;
        icon.sprite = item.Icon;
    }

    public void ClearItem() {
        thisItem = null;
        icon.sprite = null;
    }
}
