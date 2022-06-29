// Player's backpack
// Attached to the game manager and access player's loadout

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;
    private PlayerInput _playerInput;
    private PlayerController _player;
    [SerializeField] private InventoryUI _inventoryUI;
    [SerializeField] private WeaponMenu _weaponSlots;
    [SerializeField] private QuickItems _quickItemMenu;

    public delegate void InventoryEvent (int value);
    public static InventoryEvent useSimplePotion;
    public static InventoryEvent useScalingPotion;
    public static InventoryEvent updateGoldCount;
    
    private List<Item> _inventory = new List<Item>();
    private Weapons[] _weapons = new Weapons[2];

    private const int _max_inventory_size = 5;

    private int _weaponIndex = 0;
    private int _gold = 0;

    private bool _canSwitchWeapon = true;
    private WaitForSeconds _delay = new WaitForSeconds(1f);

    public int MaxSize { get { return _max_inventory_size; }}
    public bool Paused { get { return _inventoryUI.gameObject.activeSelf; }}

    void Awake()
    {
        instance = this;
        _playerInput = new PlayerInput();

        _playerInput.Player.ScrollLeft.performed += OnScrollLeft;
        _playerInput.Player.ScrollRight.performed += OnScrollRight;
        _playerInput.Player.SwitchWeapon.performed += OnSwitchWeapon;
        _playerInput.Player.Menu.performed += OnMenu;

        PlayerHealth.onDeath += OnDisable;
    }

    private void Start() {
        _player = PlayerController.instance;
        _playerInput.Enable();

        _inventory.Capacity = _max_inventory_size;
    }

    private void OnDisable() {
        _playerInput.Disable();
    }

    #region Input functions
    private void OnScrollLeft(InputAction.CallbackContext context) {
        if (!Paused && _inventory.Count > 1) {
            int index = _inventory.Count - 1;
            Item buffer = _inventory[index];

            _inventory.RemoveAt(index);
            _inventory.Insert(0, buffer);

            _quickItemMenu.RedrawSlots(_inventory);
        }
    }

    private void OnScrollRight(InputAction.CallbackContext context) {
        if (!Paused && _inventory.Count > 1) {
            Item buffer = _inventory[0];
            _inventory.RemoveAt(0);
            _inventory.Add(buffer);

            _player.SwitchItem(_inventory[0]);
            _quickItemMenu.RedrawSlots(_inventory);
        }
    }

    private void OnSwitchWeapon(InputAction.CallbackContext context) {
        if (!Paused && _canSwitchWeapon && !_player.IsAttacking) {
            _weaponIndex = Mathf.Abs(_weaponIndex - 1);
            _player.SwitchWeapon(_weapons[_weaponIndex]);
            _weaponSlots.SwitchItem();
            StartCoroutine(RecoverFromWeaponSwitch());
        }
    }

    private void OnMenu(InputAction.CallbackContext context) {
        switch (_inventoryUI.gameObject.activeSelf) {
            case (true):  // menu is open
                _inventoryUI.HideMenu();
                _inventoryUI.gameObject.SetActive(false);
                break;
            case (false):  // menu is closed 
                _inventoryUI.gameObject.SetActive(true);
                _inventoryUI.ShowMenu();
                break;
        }
    }

    private void OnUse() {
        if (Paused) {
            QuickUse(_inventoryUI.Selected);
            _inventoryUI.UpdateSlots(_inventoryUI.Selected);
        }
    }

    private void OnDrop() {
        if (Paused) { 
            RemoveItem(_inventoryUI.Selected);
            ItemGenerator.instance.PopOutObject(_player.transform.position, _inventoryUI.Selected);
            _inventoryUI.UpdateSlots(_inventoryUI.Selected);
        }
    }
    #endregion

    #region Setting Objects
    public void AddWeapon(Weapons newWeapon) {
        _weapons[_weaponIndex] = newWeapon;
        _weaponSlots.ChangeEquipment(_weaponIndex, newWeapon);
    }

    public bool AddItem(Item item) {
        if (_inventory.Count < _max_inventory_size) {
            _inventory.Add(item);

            if (_player.QuickUseSlot == null || 
                    _player.QuickUseSlot != _inventory[0]) 
            {
                _player.SwitchItem(_inventory[0]);
            }

            _quickItemMenu.RedrawSlots(_inventory);
            Debug.Log("Successfully added " + item.ItemName + " to inventory.");
            return true;
        }
        

        Debug.Log("Inventory is full!");
        return false;
    }

    public void AddGold(int amount) {
        _gold += amount;
        updateGoldCount(_gold);
    }
    #endregion

    #region Getting Objects
    // private void AccessInventory(int indexToAccess) {
    //     if (_inventory.Count > 0) {
    //         _inventoryIndex = indexToAccess % _inventory.Count;
    //         Item buffer = _inventory[0];
    //         _inventory.RemoveAt(0);
    //         _inventory.Add(buffer);

    //         _player.SwitchItem(_inventory[0]);
    //         _quickItemMenu.RedrawSlots(_inventory);
    //     }
    // }

    public bool CanAfford(int amount) {
        return (_gold - amount) > 0;
    }
    #endregion

    #region Using and Removing Items
    public void QuickUse(Item item) {
        Debug.Log("Using " + item);
        if (_inventory.Contains(item)){
            if (item is Potions pot) {
                useSimplePotion(pot._healAmount);
            }
            else if (item is PotionsScaled potScaled) {
                useScalingPotion(potScaled.healPercentage);
            }

            _inventory.RemoveAt(0);

            if (_inventory.Count > 0) _player.SwitchItem(_inventory[0]); 
            else _player.ClearItem();

            _quickItemMenu.RedrawSlots(_inventory);
        }
    }

    public void RemoveItem (Item item) {
        _inventory.Remove(item);
        _quickItemMenu.RedrawSlots(_inventory);
    }

    public void UseGold(int amount) {
        if (_gold >= amount) {
            _gold -= amount;
        }
    }

    public Item GetItemInSlot(int index) {
        if (index > _inventory.Count - 1 || _inventory[index] == null) return null;
        else return _inventory[index];
    }

    // Prevent abusing the switch weapon button
    IEnumerator RecoverFromWeaponSwitch() {
        _canSwitchWeapon = false;
        yield return _delay;
        _canSwitchWeapon = true;
    }

    #endregion
}
