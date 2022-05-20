using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemPopup : MonoBehaviour
{
    public PlayerController _player;
    private PlayerInput _playerInput;

    public GameObject _menuGUI;
    public Text _label;
    private Item _item;


    #region input action functions
    private void OnEnable() { _playerInput.Enable(); }
    private void OnDisable() { _playerInput.Disable(); }
    
    void OnPickup(InputAction.CallbackContext context) {
        if (_player == null)_player = PlayerController.instance;

        if (_player.Selection != null) {
            switch (_menuGUI.activeSelf) {
                case (false): 
                    OpenMenu();
                    break;
                case (true):
                    _player.EquipSelection();
                    CloseMenu();
                    break;
            }
        }
        
    }

    private void OnDiscard(InputAction.CallbackContext context) {
        CloseMenu();
    }

    #endregion

    private void OpenMenu() {
        // Set Item
        _item = _player.Selection?.Item;

        // Update Display
        _label.text = _item.ItemName;
        _menuGUI.SetActive(true);

        // Call Event
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
    

    void Awake() {
        _playerInput = new PlayerInput();

        _playerInput.Player.Action.performed += OnPickup;
        _playerInput.Player.Action.canceled -= OnPickup;
        _playerInput.Player.Cancel.performed += OnDiscard;
        _playerInput.Player.Cancel.canceled -= OnDiscard;

        _menuGUI.SetActive(false);
    }
}
