using R3;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using Observable = R3.Observable;

public class PlayerController : MonoBehaviour {

    public PlayerData playerData;

    private float _freeMoveSpeed;
    private float _lockOnMoveSpeed;
    private float _rotationSpeed;

    private bool _isLockedOn;

    private bool _isComboable;
    
    private Vector3 _movementDirection;
    private int _attackTriggerCount;

    private int _comboCountCheck;
    
    private CharacterController _characterController;
    
    private EventArchive _eventArchive;
    private Transform _playerCamera;
    
    private void Awake() {
    }

    void Start() {

        if(Camera.main != null) { _playerCamera = Camera.main.transform; } 
        
        _characterController = GetComponent<CharacterController>();
        
        Subscribe();
        InitializeValues();
        
        
        
        Observable.EveryValueChanged(this, _ => _attackTriggerCount)
            .Where(x => x == 1)
            .Subscribe(_ => {
                
                _eventArchive.gameplay.InvokeOnAttacking();
            });
    }

    void Update() {
        
        var camFwd = _playerCamera.forward;
        camFwd.y = 0;
        camFwd.Normalize();
        var camRight = _playerCamera.right;
        camRight.y = 0;
        camRight.Normalize();
        
        var actualDirection = camFwd * _movementDirection.z + camRight * _movementDirection.x;

        if(_movementDirection == Vector3.zero) { return; }
        var currentFwd = transform.forward;
        transform.forward = Vector3.SlerpUnclamped(currentFwd, actualDirection, _rotationSpeed * Time.deltaTime);
        
        var movementSpeed = _isLockedOn ? _lockOnMoveSpeed : _freeMoveSpeed;
        
        _characterController.Move(actualDirection * (movementSpeed * Time.deltaTime));

        
        Debug.Log(_attackTriggerCount);
    }

    private void InitializeValues() {                                                                                                                                                                       
        _freeMoveSpeed = playerData.freeMoveSpeed;
        _lockOnMoveSpeed = playerData.lockOnMoveSpeed;
        _rotationSpeed = playerData.rotationSpeed;
    }

    private void Subscribe() {
        
        _eventArchive = FindFirstObjectByType<EventArchive>();

        _eventArchive.playerInputs.OnMove += input => {

            _movementDirection = Vector3.forward * input.y + Vector3.right * input.x;
        };
        
        _eventArchive.playerInputs.OnAttack += () => { _attackTriggerCount++; };
        _eventArchive.playerInputs.OnLockOn += () => _isLockedOn = !_isLockedOn;
        
    }

    internal void ComboStart() {
        
        _isComboable = true;
        _comboCountCheck = _attackTriggerCount;
    }

    internal void ComboEnd() {
        
        if(_comboCountCheck == _attackTriggerCount) { _isComboable = false; }

        _eventArchive.gameplay.InvokeOnCombo(_isComboable);
    }

    internal void ResetAttackTriggerCounter() {

        _attackTriggerCount = 0;
        _isComboable = false;
    }
    
}
