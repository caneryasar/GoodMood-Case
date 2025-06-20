using UniRx;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public PlayerData playerData;

    private float _freeMoveSpeed;
    private float _lockOnMoveSpeed;
    private float _rotationSpeed;

    private bool _isLockedOn;
    private bool _isDummyDead;

    private bool _isComboable;
    
    private Vector3 _movementDirection;
    private int _attackTriggerCount;

    private int _comboCountCheck;
    
    private CharacterController _characterController;
    
    private EventArchive _eventArchive;
    private Transform _playerCamera;

    private Transform _dummy;
    

    void Start() {

        if(Camera.main != null) { _playerCamera = Camera.main.transform; } 
        
        _characterController = GetComponent<CharacterController>();
        
        Subscribe();
        InitializeValues();

        this.ObserveEveryValueChanged(_ => _attackTriggerCount).Subscribe(x => {

            if(x != 1 && !_isComboable) {

                _attackTriggerCount = 0;
                
                return;
            }
            _eventArchive.gameplay.InvokeOnAttacking();
        });
        
        _dummy = GameObject.FindGameObjectWithTag("Dummy").transform;
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

        if(_isDummyDead) {
            
            _isLockedOn = false;
            transform.LookAt(transform);
        }
        
        var movementSpeed = _isLockedOn ? _lockOnMoveSpeed : _freeMoveSpeed;

        if(_isLockedOn) {
            
            transform.LookAt(_dummy);
        }
        
        _characterController.Move(actualDirection * (movementSpeed * Time.deltaTime));
        
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
        _eventArchive.dummyEvents.OnDeath += () => { _isDummyDead = true; };
        _eventArchive.dummyEvents.OnRespawn += () => { _isDummyDead = false; };
        
    }

    internal void ComboStart() {
        
        _isComboable = true;
        _comboCountCheck = _attackTriggerCount;
    }

    internal void ComboEnd() {
        
        // Debug.Log($"attack start: {_comboCountCheck} / attack end: {_attackTriggerCount}");

        if(_comboCountCheck == _attackTriggerCount) { ResetAttackTriggerCounter(); }

        _eventArchive.gameplay.InvokeOnCombo(_isComboable);
    }

    internal void ResetAttackTriggerCounter() {

        _attackTriggerCount = 0;
        _isComboable = false;
    }
    
}
