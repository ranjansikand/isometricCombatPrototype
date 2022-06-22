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

    public delegate void InventoryEvent (int value);
    public static InventoryEvent useSimplePotion;
    public static InventoryEvent useScalingPotion;
    
    private List<Item> _inventory = new List<Item>();
    private Weapons[] _weapons = new Weapons[2];

    private const int _max_inventory_size = 5;

    private int _inventoryIndex = 0;
    private int _weaponIndex = 0;
    private int _gold = 0;

    private bool _canSwitchWeapon = true;
    private WaitForSeconds _delay = new WaitForSeconds(1f);

    void Awake()
    {
        instance = this;
        _playerInput = new PlayerInput();

        _playerInput.Player.ScrollLeft.performed += OnScrollLeft;
        _playerInput.Player.ScrollRight.performed += OnScrollRight;
        _playerInput.Player.SwitchWeapon.performed += OnSwitchWeapon;

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
        AccessInventory(Mathf.Max(0, _inventoryIndex - 1));
    }

    private void OnScrollRight(InputAction.CallbackContext context) {
        AccessInventory(Mathf.Min(_inventoryIndex + 1, 5));
    }

    private void OnSwitchWeapon(InputAction.CallbackContext context) {
        if (_canSwitchWeapon && !_player.IsAttacking) {
            _weaponIndex = Mathf.Abs(_weaponIndex - 1);
            _player.SwitchWeapon(_weapons[_weaponIndex]);
            StartCoroutine(RecoverFromWeaponSwitch());
        }
    }
    #endregion

    #region Setting Objects
    public void AddWeapon(Weapons newWeapon) {
        _weapons[_weaponIndex] = newWeapon;
    }

    public bool AddItem(Item item) {
        if (_inventory.Count < _max_inventory_size) {
            _inventory.Add(item);

            if (_player.QuickUseSlot == null || 
                    _player.QuickUseSlot != _inventory[_inventoryIndex]) 
            {
                _inventoryIndex = _inventory.IndexOf(item);
                _player.SwitchItem(item);
            }

            Debug.Log("Successfully added " + item.ItemName + " to inventory.");
            return true;
        }
        

        Debug.Log("Inventory is full!");
        return false;
    }

    public void AddGold(int amount) {
        _gold += amount;
        Debug.Log("Added " + amount + " gold (" + _gold + " total)");
    }
    #endregion

    #region Getting Objects
    private void AccessInventory(int indexToAccess) {
        if (_inventory[indexToAccess] != null) {
            _inventoryIndex = indexToAccess;
            _player.SwitchItem(_inventory[_inventoryIndex]);
        }
    }

    private void GetNextItemIndex() {
        if (_inventory.Count > 1) {
            if (_inventory[_inventoryIndex+1] != null) {
                _inventoryIndex+=1;
            }
        } else {
            _inventoryIndex = 0;
        }
    }

    public bool CanAfford(int amount) {
        return (_gold - amount) > 0;
    }
    #endregion

    #region Using and Removing Items
    public void QuickUse(Item item) {
        if (_inventory.Contains(item)){
            if (item is Potions pot) {
                useSimplePotion(pot._healAmount);
            }
            else if (item is PotionsScaled potS) {
                useScalingPotion(potS.healPercentage);
            }

            RemoveItem(item);
            GetNextItemIndex();
            if (_inventory.Count > 0) {
                _player.SwitchItem(_inventory[_inventoryIndex]);
            } else {
                _player.ClearItem();
            }
        }
    }

    public void RemoveItem (Item item) {
        _inventory.Remove(item);
    }

    public void UseGold(int amount) {
        if (_gold >= amount) {
            _gold -= amount;
        }
    }

    // Prevent abusing the switch weapon button
    IEnumerator RecoverFromWeaponSwitch() {
        _canSwitchWeapon = false;
        yield return _delay;
        _canSwitchWeapon = true;
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        
    }
}
