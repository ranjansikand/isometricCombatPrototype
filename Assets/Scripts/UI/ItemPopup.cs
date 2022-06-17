using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemPopup : MonoBehaviour
{
    public PlayerController _player;
    private PlayerInput _playerInput;

    public GameObject _menuGUI, _damageSlot;
    public Text _label, _description, _stats;
    public Image _icon;

    private Item _item;


    private bool _isDead;

    #region input action functions
    private void OnEnable() { _playerInput.Enable(); }
    private void OnDisable() { _playerInput.Disable(); }
    
    private void OnPickup(InputAction.CallbackContext context) {
        if (_player == null)_player = PlayerController.instance;

        if (_player.Selection != null) {
            switch (_menuGUI.activeSelf) {
                case (false): 
                    OpenMenu();
                    break;
                case (true):
                    EquipOrUseItem();
                    CloseMenu();
                    break;
            }
        }
        
    }

    private void OnDiscard(InputAction.CallbackContext context) {
        CloseMenu();
    }

    #endregion
    #region clickable UI buttons
    public void EquipButtonClicked() {
        if (_isDead) return;
        EquipOrUseItem();
        CloseMenu();
    }

    public void CancelButtonClicked() {
        if (_isDead) return;
        CloseMenu();
    }
    #endregion

    private void OpenMenu() {
        // Set Item
        _item = _player.Selection?.Item;

        // Update Display
        _label.text = _item.ItemName;
        _description.text = _item.ItemDescription;
        _icon.sprite = _item.Icon;

        // Update stats in menu
        if (_item is Weapons wep) {
            _damageSlot.SetActive(true);
            _stats.text = wep._damage.ToString();
        }
        else if (_item is Potions pot) {
            _damageSlot.SetActive(true);
            _stats.text = pot._healAmount.ToString();
        }
        else {
            _damageSlot.SetActive(false);
        }

        // Open menus
        _menuGUI.SetActive(true);
        _player.MenuOpen = true;
    }

    private void CloseMenu () {
        // Remove item
        _player.DeselectItem();
        _item = null;

        // Update Display
        _menuGUI.SetActive(false);

        // Call Event
        _player.MenuOpen = false;
    }

    private void EquipOrUseItem() {
        if (_item.IsEquippable()) {
            _player.EquipSelection();
        }
        else {
            Debug.Log("Add item to inventory");
        }
    }
    

    void Awake() {
        _playerInput = new PlayerInput();

        _playerInput.Player.Action.performed += OnPickup;
        _playerInput.Player.Action.canceled -= OnPickup;
        _playerInput.Player.Cancel.performed += OnDiscard;
        _playerInput.Player.Cancel.canceled -= OnDiscard;

        PlayerHealth.onDeath += OnDisable;

        _menuGUI.SetActive(false);
    }
}
