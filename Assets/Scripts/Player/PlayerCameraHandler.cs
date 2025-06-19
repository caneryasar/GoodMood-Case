using System;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerCameraHandler : MonoBehaviour {
    
    private CinemachineCamera _playerCamera;

    [SerializeField] private float freeLookFOV;
    [SerializeField] private float lockOnFOV;
    
    private Transform _dummy;
    
    private bool _freeLookCameraActive = true;

    private bool _isDummyDead = false;
    
    private EventArchive _eventArchive;

    private void Awake() {

        _eventArchive = FindFirstObjectByType<EventArchive>();

        _eventArchive.playerInputs.OnLockOn += SetUpCameras;
        _eventArchive.gameplay.OnDummyHit += CameraShake;
        _eventArchive.dummyEvents.OnDeath += DummyDeath;
        _eventArchive.dummyEvents.OnRespawn += () => _isDummyDead = false;

        _playerCamera = GetComponentInChildren<CinemachineCamera>();
    }

    private void DummyDeath() {
        
        _isDummyDead = true;
        
        _freeLookCameraActive = true;
        _playerCamera.Lens.FieldOfView = freeLookFOV;
        _playerCamera.LookAt = transform;
    }

    private void CameraShake() {
        //todo: shake camera based on combo
    }

    private void SetUpCameras() {

        if(_isDummyDead) { return; }

        _freeLookCameraActive = !_freeLookCameraActive;
        
        _playerCamera.Lens.FieldOfView = _freeLookCameraActive ? freeLookFOV : lockOnFOV;
        _playerCamera.LookAt = _freeLookCameraActive ? transform : _dummy;
    }

    void Start() {

        _dummy = GameObject.FindGameObjectWithTag("Dummy").transform;
        
        _freeLookCameraActive = true;
    }
}