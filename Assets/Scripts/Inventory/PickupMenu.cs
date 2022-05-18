using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PickupMenu : MonoBehaviour
{
    private PlayerController _player;
    private PlayerInput _playerInput;

    public GameObject _menuGUI;
    public Text _label;

    public delegate void PickupMenuAction (Item item);
    public static PickupMenuAction menuOpened, itemEquip, discardSelection;

    WaitForSeconds _menuToggleDelay = new WaitForSeconds(0.1f);


    #region input action functions

    private void OnEnable() { _playerInput.Enable(); }
    private void OnDisable() { _playerInput.Disable(); }
    
    void OnPickup(InputAction.CallbackContext context) {
        if (PlayerController.instance.SelectedItem != null) {
            Item item = PlayerController.instance.SelectedItem;

            switch (_menuGUI.activeSelf) {
                case (false): OpenMenu(item); break;
                case (true): EquipItem(item); break;
            }
        }
    }

    private void OnDiscard(InputAction.CallbackContext context) {
        DiscardItem();
    }

    #endregion

    #region private methods

    private void OpenMenu(Item item) {
        _menuGUI.SetActive(true);
        _label.text = item.ItemName;
        menuOpened(item);  // call event

        if (item is Weapons wep) {
            // Weapon Specific action
        }
    }

    private void CloseMenu () {
        _menuGUI.SetActive(false);
    }

    // Below are called by member-functions and UI buttons
    public void DiscardItem() {
        discardSelection(null);  // call event
        Invoke(nameof(CloseMenu), 0.1f);
    }

    public void EquipItem(Item item = null) {
        Item passItem = item != null ? item : PlayerController.instance.SelectedItem;
        itemEquip(passItem);  // call event
        DiscardItem();
    }
    
    #endregion
    

    void Awake() {
        _player = PlayerController.instance;
        _playerInput = new PlayerInput();

        _playerInput.Player.Action.performed += OnPickup;
        _playerInput.Player.Action.canceled -= OnPickup;
        _playerInput.Player.Cancel.performed += OnDiscard;
        _playerInput.Player.Cancel.canceled -= OnDiscard;

        _menuGUI.SetActive(false);
    }
}
