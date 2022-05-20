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
    private int _attack1, _attack2, _finisher;

    // Conditions
    private bool _needToSwitchToIdle = false;
    private bool _lockedIntoState = false;
    private bool _menuOpen = false;

    // Scriptable Objects
    [Header("Items only shown in inspector for debugging")]
    [SerializeField] private Loot _selection;
    [SerializeField] private Weapons _mainWeapon;
    [SerializeField] private Talismans _equippedTalisman;
    #endregion

    #region getters and setters
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; }}
    public Animator Animator { get { return _animator; }}
    public CharacterController CharacterController { get { return _characterController; }}

    public Loot Selection { get { return _selection; }}
    public Weapons MainWeapon { get { return _mainWeapon; }}
    public Talismans EquippedTalisman { get { return _equippedTalisman; }}

    public Coroutine ComboResetRoutine { get { return _comboResetRoutine; } set { _comboResetRoutine = value; }}

    public WaitForSeconds ComboResetTimer { get { return _comboResetTimer; }}

    public bool NeedToSwitchToIdle { set { _needToSwitchToIdle = value; }}
    public bool MenuOpen { set { _menuOpen = value; }}

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
    }

    private void EndAction() {
        _lockedIntoState = false;

        if (_currentMovementInput == Vector2.zero) _states.SwitchState(_states.GetState(3));
        else _states.SwitchState(_states.GetState(0));
    }

    #endregion

    #region Inventory-Based Functions
    public void DeselectItem() {
        _selection?.DeselectObject();
        _selection = null;
    }

    public void EquipSelection() {
        Item waste = null;

        if (_selection.Item is Weapons wep) {
            waste = _mainWeapon;
            _mainWeapon = wep;
        }
        else if (_selection.Item is Talismans tal) {
            waste = _equippedTalisman;
            _equippedTalisman = tal;
        }

        if (waste != null) {
            GameObject buffer = ItemGenerator.instance.SpawnObject(transform.position, waste);
            buffer.GetComponent<Rigidbody>().AddForce(
                Random.Range(-3f, 3f), 2f, 
                Random.Range(-3f, 3f), 
                ForceMode.Impulse);
        }

        GenerateAnimationHashes();
        Destroy(_selection.gameObject);
    }
    #endregion

    #region Other functions
    private void GenerateAnimationHashes() {  // Call on Equip or Unequip
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
    }

    public bool CanMoveForward() {
        return Physics.Raycast(
            transform.position + Vector3.up, 
            _appliedMovement,
            0.5f
        );
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

        _standardDodgeHash = Animator.StringToHash("Dodge");
        _standardAttackHash = Animator.StringToHash("Hit");
        GenerateAnimationHashes();

        _playerInput.Player.Movement.performed += OnMovementInput;
        _playerInput.Player.Movement.canceled += OnMovementInput;
        _playerInput.Player.Dodge.performed += OnDodge;
        _playerInput.Player.Attack.performed += OnAttack;

        ActionState.onAnimationComplete += EndAction;
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
    }

    void OnTriggerExit(Collider other) {
        if (!_menuOpen && _selection != null && _selection == other.GetComponent<Loot>()) {
            DeselectItem();
        }
    }
}
