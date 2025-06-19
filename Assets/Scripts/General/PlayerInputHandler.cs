using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour {

    private EventArchive _eventArchive;
    
    private InputAction _move;
    private InputAction _attack;
    private InputAction _lockOn;

    private bool _isPlayable;
    
    private PlayerInput _playerInput;
    
    private void Awake() {

        _eventArchive = FindFirstObjectByType<EventArchive>();
        
        _playerInput = GetComponent<PlayerInput>();
        
        _move = InputSystem.actions.FindAction("Move");
        _attack = InputSystem.actions.FindAction("Attack");
        _lockOn = InputSystem.actions.FindAction("LockOn");
    }

    void Start() {

        _isPlayable = true;

        _playerInput.camera = Camera.main;
    }

    // Update is called once per frame
    void Update() {
        
        if(!_isPlayable) { return; }
        
        _eventArchive.playerInputs.InvokeOnMove(_move.ReadValue<Vector2>());
        if(_attack.triggered) { _eventArchive.playerInputs.InvokeOnAttack(); }
        if(_lockOn.triggered) { _eventArchive.playerInputs.InvokeOnLockOn(); }
    }
}
