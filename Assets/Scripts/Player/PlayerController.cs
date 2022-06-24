using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    #region variables
    public static PlayerController instance;

    private PlayerInput _playerInput;
    private CharacterController _characterController;
    private Animator _animator;
    private PlayerInventory _inventory;

    // events
    public delegate void PlayerEvent(int value);
    public static PlayerEvent keyCount;
    public static PlayerEvent maxHealthUpdate;

    // State Machine
    private PlayerBaseState _currentState;
    private PlayerStateFactory _states;

    // Movement
    private float _walkSpeed = 4.5f;
    private float _dodgeSpeed = 1.5f;
    private Vector2 _currentMovementInput;
    private Vector3 _appliedMovement;

    // Attacking
    public int _baseDamage;
    private Coroutine _comboResetRoutine = null;
    private int _attackNumber = 1;
    private WaitForSeconds _comboResetTimer = new WaitForSeconds(1.5f);

    // Animations
    private int _standardIdleHash;
    private int _standardHurtHash;
    private int _standardRunHash;
    private int _standardDodgeHash;
    private int _standardAttackHash;
    private int _attack1, _attack2, _finisher;
    private int _dirX, _dirY;

    // Conditions
    private bool _needToSwitchToIdle = false;
    private bool _lockedIntoState = false;
    private bool _attacking = false;
    private bool _menuOpen = false;

    // Inventory-based Scriptable Objects
    private Loot _selection;
    private Weapons _mainWeapon;
    private Talismans _equippedTalisman;
    private Item _quickUseSlot;

    private int _numberOfKeys = 0;

    private GameObject _equippedWeapon = null;  // weapon currently in use
    [SerializeField] private Transform _hand;  // weapon spawnpoint
    [SerializeField] private LayerMask layerMask;  // layer for ray
    #endregion

    #region getters and setters
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; }}
    public Animator Animator { get { return _animator; }}
    public CharacterController CharacterController { get { return _characterController; }}

    public Loot Selection { get { return _selection; }}
    public Weapons MainWeapon { get { return _mainWeapon; }}
    public Talismans EquippedTalisman { get { return _equippedTalisman; }}
    public Item QuickUseSlot { get { return _quickUseSlot; }}

    public Coroutine ComboResetRoutine { get { return _comboResetRoutine; } set { _comboResetRoutine = value; }}

    public WaitForSeconds ComboResetTimer { get { return _comboResetTimer; }}

    public bool NeedToSwitchToIdle { set { _needToSwitchToIdle = value; }}
    public bool MenuOpen { set { _menuOpen = value; }}
    public bool IsAttacking { get { return _attacking; }}

    public int StandardIdleHash { get { return _standardIdleHash; }}
    public int StandardRunHash { get { return _standardRunHash; }}
    public int StandardDodgeHash { get { return _standardDodgeHash; }}
    public int StandardAttackHash { get {return _standardAttackHash; }}
    public int Attack1 { get { return _attack1; }}
    public int Attack2 { get { return _attack2; }}
    public int Finisher { get { return _finisher; }}
    public int AttackNumber { get { return _attackNumber; } set { _attackNumber = value; }}

    public float WalkSpeed { get { return _walkSpeed; }}
    public float DodgeSpeed { get { return _dodgeSpeed; }}

    public Vector2 CurrentMovementInput { get { return _currentMovementInput; }}
    public Vector3 AppliedMovement { get { return _appliedMovement; } set { _appliedMovement = value; }}
    #endregion

    #region input functions

    private void OnDisable() {
        _playerInput.Disable();
    }
    private void OnMovementInput(InputAction.CallbackContext context) {
        _currentMovementInput = context.ReadValue<Vector2>().normalized;
        if (!_lockedIntoState && _currentMovementInput != Vector2.zero) {
            _states.SwitchState(_states.GetState(0));
        }
    }

    private void OnDodge(InputAction.CallbackContext context) {
        if (!_lockedIntoState) {
            _states.SwitchState(_states.GetState(1));
            _lockedIntoState = true;
        }
    }

    private void OnAttack(InputAction.CallbackContext context) {
        if (!_lockedIntoState) {
            _states.SwitchState(_states.GetState(2));
            _lockedIntoState = true;
            _attacking = true;
        }
    }
    
    private void OnUseItem(InputAction.CallbackContext context) {
        if (!PlayerInventory.instance.Paused) PlayerInventory.instance.QuickUse(_quickUseSlot);
    }

    private void OnDeath() {
        _states.SwitchState(_states.GetState(4));
    }

    public void OnHurt() {
        if (!_lockedIntoState) {
            _animator.Play("Hurt");
        }
    }
    #endregion

    #region Event functions
    private void EndAction() {
        _lockedIntoState = false;
        _attacking = false;

        if (_currentMovementInput == Vector2.zero) _states.SwitchState(_states.GetState(3));
        else _states.SwitchState(_states.GetState(0));
    }

    private void UpdateBaseDamage(int might) {
        _baseDamage = might;
    }
    #endregion

    #region Inventory-Based Functions
    //  Functions that interface with the pickup menu
    public void DeselectItem() {
        _selection?.DeselectObject();
        _selection = null;
    }

    public void EquipSelection() {
        Item waste = null;

        if (!_selection.Item.IsEquippable()) {
            if (_inventory.AddItem(_selection.Item)) {
                Destroy(_selection.gameObject);
            }
            else {
                Debug.Log("Unable to add to inventory");
            }
            return;
        }

        if (_selection.Item is Weapons wep) {
            waste = _mainWeapon;
            _mainWeapon = wep;
            _inventory.AddWeapon(wep);
        }
        else if (_selection.Item is Talismans tal) {
            if (_equippedTalisman != null) 
                maxHealthUpdate(-_equippedTalisman._maxHealthChange);
            waste = _equippedTalisman;
            _equippedTalisman = tal;
        }

        if (waste != null) ItemGenerator.instance.PopOutObject(transform.position, waste);

        UpdateEquipmentStats();
        Destroy(_selection.gameObject);
    }

    // Functions that interface with inventory
    public void SwitchWeapon(Weapons weapon) {
        _mainWeapon = weapon;
        UpdateEquipmentStats();
    }

    public void SwitchItem(Item item = null) { _quickUseSlot = item; Debug.Log("Quick item = " + item);}

    public void ClearItem () { _quickUseSlot = null; }

    public void AddKey() { 
        _numberOfKeys++; 
        keyCount(_numberOfKeys);
    }

    public bool HasKey() {
        switch (_numberOfKeys > 0) {
            case (true): 
                _numberOfKeys--;
                keyCount(_numberOfKeys);
                return true;
            case (false): return false;
        }
    }
    #endregion

    #region Other functions
    private void UpdateEquipmentStats() {  // Call on Equip or Unequip
        // Animation Updates
        _standardIdleHash = _mainWeapon?._idle == null ? 
            Animator.StringToHash("Idle") : 
            Animator.StringToHash(_mainWeapon._idle.name);
        _standardRunHash = _mainWeapon?._run == null ? 
            Animator.StringToHash("Run") : 
            Animator.StringToHash(_mainWeapon._run.name);
        if (_mainWeapon != null) {
            _attack1 = Animator.StringToHash(_mainWeapon._attack1.name);
            _attack2 = Animator.StringToHash(_mainWeapon._attack2.name);
            _finisher = Animator.StringToHash(_mainWeapon._finisher.name);
        }

        // Visual equipment update
        if (_mainWeapon?._weapon != null) {
            if (_equippedWeapon != null) Destroy(_equippedWeapon);

            _equippedWeapon = Instantiate(_mainWeapon?._weapon, _hand.position, _hand.rotation, _hand);
            
            var wep = _equippedWeapon.GetComponentInChildren<PlayerWeapon>();
            wep.SetDamage(_mainWeapon._damage + _baseDamage);
            wep.SetKnockback(_mainWeapon._knockback);
        }

        if (_mainWeapon == null && _equippedWeapon != null) Destroy(_equippedWeapon);

        // Update stats
        if (_equippedTalisman != null) {
            maxHealthUpdate(_equippedTalisman._maxHealthChange);
        }
    }

    public bool CanMoveForward() {
        return !Physics.Raycast(
            transform.position + Vector3.forward, 
            _appliedMovement,
            0.5f,
            layerMask
        );
    }
    #endregion

    // Runs first
    private void Awake() {
        if (instance != null) {
            Debug.LogWarning("Multiple players found!");
            return;
        }
        instance = this;

        _playerInput = new PlayerInput();    
    }

    // Runs after Awake
    private void OnEnable()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _states = new PlayerStateFactory(this);
        _inventory = PlayerInventory.instance;

        _states.GenerateStates();
        _states.SwitchState(_states.GetState(0));

        _standardDodgeHash = Animator.StringToHash("Dodge");
        _standardAttackHash = Animator.StringToHash("Hit");
        _standardHurtHash = Animator.StringToHash("Hurt");
        _dirX = Animator.StringToHash("dirX");
        _dirY = Animator.StringToHash("dirY");
        UpdateEquipmentStats();

        _playerInput.Player.Movement.performed += OnMovementInput;
        _playerInput.Player.Movement.canceled += OnMovementInput;
        _playerInput.Player.Dodge.performed += OnDodge;
        _playerInput.Player.Attack.performed += OnAttack;
        _playerInput.Player.UseItem.performed += OnUseItem;

        ActionState.onAnimationComplete += EndAction;
        PlayerHealth.onDeath += OnDeath;
        PlayerHealth.onDeath += OnDisable;
        _playerInput.Enable();
    }

    void Update()
    {
        _currentState.UpdateState();

        if (_needToSwitchToIdle) {
            _states.SwitchState(_states.GetState(3));
        }
    }

    void OnTriggerEnter(Collider other) {
        if (_selection == null && !_menuOpen && other.gameObject.layer == 6) {
            _selection = other.GetComponent<Loot>();
            _selection?.SelectObject();
        }

        if (other.gameObject.layer == 10) {
            AddKey();
            Destroy(other.gameObject);
        }

        if (other.gameObject.layer == 8) {
            var dir = (transform.position - other.gameObject.transform.position).normalized;
            _animator.SetFloat(_dirX, dir.x);
            _animator.SetFloat(_dirY, dir.z);
        }
    }

    void OnTriggerExit(Collider other) {
        if (!_menuOpen && _selection != null && _selection == other.GetComponent<Loot>()) {
            DeselectItem();
        }
    }
}
