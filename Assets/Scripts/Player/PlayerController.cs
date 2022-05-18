using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class PlayerController : MonoBehaviour, IReceivable
{
    #region variables
    public static PlayerController instance;

    private PlayerInput _playerInput;
    private CharacterController _characterController;
    private Animator _animator;

    // State Machine
    private PlayerBaseState _currentState;
    private PlayerStateFactory _states;

    // Movement
    private float _walkSpeed = 2.5f;
    private float _dodgeSpeed = 2.5f;
    private Vector2 _currentMovementInput;
    private Vector3 _appliedMovement;

    // Attacking
    private Coroutine _comboResetRoutine = null;
    private int _attackNumber = 1;
    private WaitForSeconds _comboResetTimer = new WaitForSeconds(1.5f);

    // Animations
    private int _standardIdleHash;
    private int _standardRunHash;
    private int _standardDodgeHash;
    private int _standardAttackHash;

    // Conditions
    private bool _needToSwitchToIdle = false;
    private bool _lockedIntoState = false;

    // Scriptable Objects
    [Header("Items only shown in inspector for debugging")]
    [SerializeField] private Weapons _mainWeapon;
    [SerializeField] private Talismans _equippedTalisman;
    [SerializeField] private Item _selectedItem;
    #endregion

    #region getters and setters
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; }}
    public Animator Animator { get { return _animator; }}
    public CharacterController CharacterController { get { return _characterController; }}

    public Weapons MainWeapon { get { return _mainWeapon; }}
    public Talismans EquippedTalisman { get { return _equippedTalisman; }}
    public Item SelectedItem { get { return _selectedItem; }}

    public Coroutine ComboResetRoutine { get { return _comboResetRoutine; } set { _comboResetRoutine = value; }}

    public WaitForSeconds ComboResetTimer { get { return _comboResetTimer; }}

    public bool NeedToSwitchToIdle { get { return _needToSwitchToIdle; } set { _needToSwitchToIdle = value; }}

    public int StandardIdleHash { get { return _standardIdleHash; }}
    public int StandardRunHash { get { return _standardRunHash; }}
    public int StandardDodgeHash { get { return _standardDodgeHash; }}
    public int StandardAttackHash { get {return _standardAttackHash; }}
    public int AttackNumber { get { return _attackNumber; } set { _attackNumber = value; }}

    public float WalkSpeed { get { return _walkSpeed; }}
    public float DodgeSpeed { get { return _dodgeSpeed; }}

    public Vector2 CurrentMovementInput { get { return _currentMovementInput; }}
    public Vector3 AppliedMovement { get { return _appliedMovement; } set { _appliedMovement = value; }}
    public Vector3 CurrentPosition { get { return transform.position; }}
    #endregion

    #region input and event callback functions
    private void OnEnable() {
        _playerInput.Enable();
    }

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
        }

        // For testing
        ItemGenerator.instance.SpawnObject (transform.position + new Vector3(5, 1, 5));
    }

    public void EndAction() {
        _lockedIntoState = false;

        if (_currentMovementInput == Vector2.zero) _states.SwitchState(_states.GetState(3));
        else _states.SwitchState(_states.GetState(0));
    }

    #endregion

    #region Inventory-Based Functions

    public void SelectItem(Item newItem = null) {
        _selectedItem = newItem;
    }

    void DiscardSelection(Item item) {
        _selectedItem = null;
    }

    void EquipItem(Item item) {
        if (item is Weapons wep) {
            if (_mainWeapon != null) {
                Inventory.instance.AddItem(_mainWeapon);
            }
            _mainWeapon = wep;
        } else if (item is Talismans talisman) {
            if (_mainWeapon != null) {
                Inventory.instance.AddItem(_equippedTalisman);
            }
            _equippedTalisman = talisman;
        }
    }

    #endregion

    void Awake()
    {
        if (instance != null) {
            Debug.LogWarning("Multiple players found!");
            return;
        }
        instance = this;

        _playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _states = new PlayerStateFactory(this);

        _states.GenerateStates();
        _states.SwitchState(_states.GetState(0));

        _standardIdleHash = Animator.StringToHash("Idle");
        _standardRunHash = Animator.StringToHash("Run");
        _standardDodgeHash = Animator.StringToHash("Dodge");
        _standardAttackHash = Animator.StringToHash("Hit");

        _playerInput.Player.Movement.performed += OnMovementInput;
        _playerInput.Player.Movement.canceled += OnMovementInput;
        _playerInput.Player.Dodge.performed += OnDodge;
        _playerInput.Player.Attack.performed += OnAttack;

        ActionState.onAnimationComplete += EndAction;
        PickupMenu.itemEquip += EquipItem;
        PickupMenu.discardSelection += DiscardSelection;
    }

    void Update()
    {
        _currentState.UpdateState();

        if (_needToSwitchToIdle) {
            _states.SwitchState(_states.GetState(3));
        }
    }
}
