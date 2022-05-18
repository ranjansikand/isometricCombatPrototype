// Script attached to item prefab
// Item assigned by Generator

using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ItemScript : MonoBehaviour
{
    private Item item; 

    private bool _isSelected = false;
    private bool _menuOpen = false;

    public GameObject _selectedHighlight;
    public Item Item { set { item = value; }}

    #region helper functions
    void ToggleSelection (bool status) {
        _isSelected = status;
        _selectedHighlight?.SetActive(status);
    }

    void TriggerActions(Collider given, bool status)
    {
        Item temp = status ? item : null;
        
        var itemReceiver = given.GetComponent<IReceivable>();
        
        if (itemReceiver != null) {
            itemReceiver?.SelectItem(temp);
            ToggleSelection(status);
        }
    }

    #endregion

    #region event callbacks

    void OnEquip(Item inItem) {
        if (_menuOpen) Destroy(gameObject);
    }

    void MenuOpened(Item item) { 
        _menuOpen = _isSelected;
    }

    void OnDiscard(Item item) { 
        _isSelected = _menuOpen = false;
        ToggleSelection(false);
    }

    #endregion

    void Awake()
    {
        PickupMenu.itemEquip += OnEquip;
        PickupMenu.menuOpened += MenuOpened;
        PickupMenu.discardSelection += OnDiscard;

        ToggleSelection(false);
    }

    void OnTriggerEnter(Collider collider) {
        TriggerActions(collider, true);
    }

    void OnTriggerExit(Collider collider) {
        TriggerActions(collider, _menuOpen);
    }
}
