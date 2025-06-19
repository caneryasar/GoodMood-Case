using System;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class DummyUIHandler : MonoBehaviour {

    [SerializeField] private Image healthFill;
    
    private Camera _mainCamera;
    
    private Transform _canvas;

    private bool _isCanvasOpen = false;
    private bool _isDead = false;
    
    private EventArchive _eventArchive;
    
    void Start() {
        
        _canvas = GetComponentInChildren<Canvas>().transform;
        
        _canvas.gameObject.SetActive(_isCanvasOpen);
        _eventArchive = FindFirstObjectByType<EventArchive>();

        _eventArchive.dummyEvents.OnHealthChanged += ShowHealth;
        _eventArchive.playerInputs.OnLockOn += OpenCloseCanvas;
        _eventArchive.dummyEvents.OnDeath += Dead;
        _eventArchive.dummyEvents.OnRespawn += () => _isDead = false;
        
        if(_mainCamera !=null) { _mainCamera = Camera.main; }

        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        
        this.LateUpdateAsObservable().Subscribe(_ => {

            _canvas.LookAt(transform.position + _mainCamera.transform.rotation * Vector3.forward, _mainCamera.transform.rotation * Vector3.up);
        });
    }

    private void Dead() {

        _isDead = true;
        
        _isCanvasOpen = false;
        _canvas.gameObject.SetActive(false);
    }

    private void OpenCloseCanvas() {

        if(_isDead) { return; }
        
        _isCanvasOpen = !_isCanvasOpen;
        
        _canvas.gameObject.SetActive(_isCanvasOpen);
    }

    private void ShowHealth(float current, float start) {
        
        var currentFill = healthFill.fillAmount;
        var targetFill = current / start;

        DOVirtual.Float(currentFill, targetFill, .5f, fill => { healthFill.fillAmount = fill; });
    }

    private void Update() {
        
        Debug.Log(_isCanvasOpen);
    }
}