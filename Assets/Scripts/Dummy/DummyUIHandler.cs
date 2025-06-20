using System;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class DummyUIHandler : MonoBehaviour {

    [SerializeField] private Image healthFill;
    
    private TextMeshProUGUI _dmgText;
    private Camera _mainCamera;
    
    private Canvas _canvas;

    private bool _isCanvasOpen = false;
    private bool _isDead = false;
    
    private EventArchive _eventArchive;
    
    void Start() {
        
        _canvas = GetComponentInChildren<Canvas>();
        _dmgText = GetComponentInChildren<TextMeshProUGUI>();
        _canvas.gameObject.SetActive(_isCanvasOpen);
        
        Subscribe();
        
        if(_mainCamera !=null) { _mainCamera = Camera.main; }

        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        
        _dmgText.gameObject.SetActive(false);
        
        this.LateUpdateAsObservable().Subscribe(_ => {

            _canvas.transform.LookAt(_canvas.transform.position + _mainCamera.transform.rotation * Vector3.forward, _mainCamera.transform.rotation * Vector3.up);
        });
    }

    private void Subscribe() {
        
        _eventArchive = FindFirstObjectByType<EventArchive>();

        _eventArchive.dummyEvents.OnHealthChanged += ShowHealth;
        _eventArchive.playerInputs.OnLockOn += OpenCloseCanvas;
        _eventArchive.dummyEvents.OnDeath += Dead;
        _eventArchive.dummyEvents.OnRespawn += () => _isDead = false;
        _eventArchive.dummyEvents.OnGetDamage += ShowDamage;
        
    }

    private void ShowDamage(float dmg) {
        
        var textBox = _dmgText.rectTransform;
        
        textBox.gameObject.SetActive(false);
        _dmgText.alpha = 1f;
        
        _dmgText.text = $"{dmg}";

        var randomPos = new Vector2(Random.Range(-500f, 500f), Random.Range(-500f, 500f));

        textBox.anchoredPosition = randomPos;
        textBox.localRotation = Quaternion.Euler(0, 0, 1 * Random.Range(-10f, 10f));

        textBox.gameObject.SetActive(true);

        var normalized = new Vector2(textBox.up.normalized.x, textBox.up.normalized.y);
        
        DOVirtual.Vector2(randomPos, randomPos + normalized * Random.Range(1f, 3f), 1f, pos => textBox.anchoredPosition = pos);
        
        textBox.DOScale(textBox.localScale * Random.Range(1f, 2f), 1f);
        _dmgText.DOFade(0f, 1f).OnComplete(() => textBox.gameObject.SetActive(false));
        
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
}