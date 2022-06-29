
using UnityEngine;

public class WeaponMenu : MonoBehaviour
{
    [SerializeField] WeaponSlot[] _weapons;

    private int _selectedSlot = 1;  // Switch to 0 on Awake

    void Awake() {
        SwitchItem();
    }

    public void ChangeEquipment(int slot, Item item = null) {
        if (item.ItemName != null) _weapons[slot].SetItem(item);
        else _weapons[slot].DropItem();
    }

    public void SwitchItem() {
        int nextWep = _selectedSlot == 1 ? 0 : 1;
        
        _weapons[_selectedSlot].Deselect();
        _weapons[nextWep].Select();

        _selectedSlot = nextWep;
    }
}
