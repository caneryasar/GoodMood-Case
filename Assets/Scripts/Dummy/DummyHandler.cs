using System;
using DG.Tweening;
using Microsoft.Win32.SafeHandles;
using UniRx;
using UniRx.Triggers;
using Unity.Cinemachine;
using UnityEngine;

public class DummyHandler : MonoBehaviour {

    [SerializeField] private float dummyHealth = 100;

    private float _startHealth;
    private int _comboCount;
    
    private CapsuleCollider _collider;
    private Animator _animator;
    private CinemachineImpulseSource _impulseSource;
    private SkinnedMeshRenderer _renderer;
    private Material _material;
    
    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Dead = Animator.StringToHash("Dead");
    
    private EventArchive _eventArchive;

    void Start() {
        
        _startHealth = dummyHealth;

        _collider = GetComponentInChildren<CapsuleCollider>();
        _animator = GetComponent<Animator>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _material = _renderer.material;
        
        _eventArchive = FindFirstObjectByType<EventArchive>();

        _eventArchive.dummyEvents.OnGetDamage += ReceiveDamage;
        _eventArchive.gameplay.OnAttackComboCount += count => _comboCount = count;
        
    }

    private void OnTriggerEnter(Collider other) {

        if(other.CompareTag("Sword")) {

            _eventArchive.gameplay.InvokeOnDummyHit();
            _impulseSource.GenerateImpulseWithForce(0.5f * _comboCount);

        }
    }

    private void ReceiveDamage(float dmg) {

        if(dmg > dummyHealth) {

            dummyHealth = 0;
            
            DeathAnimation();
            _eventArchive.dummyEvents.InvokeOnDeath();
        }
        else {
            
            dummyHealth -= dmg;
            HitAnimation();
        }
        
        _eventArchive.dummyEvents.InvokeOnHealthChanged(dummyHealth, _startHealth);
    }

    private void HitAnimation() {
        
        _animator.SetBool(Hit, true);
        _animator.SetBool(Idle, false);

        DOVirtual.DelayedCall(.2f, () => {
            
            _animator.SetBool(Hit, false);
            _animator.SetBool(Idle, true);
        });
    }

    private void DeathAnimation() {
        
        _collider.enabled = false;
        
        _animator.SetBool(Idle, false);
        
        _animator.SetBool(Dead, true);

        DOVirtual.Float(0f, 1f, 2.5f, dissolve => _material.SetFloat("_DissolveAmount", dissolve));
        
        Respawn();
    }

    private void Respawn() {
        
        DOVirtual.DelayedCall(4f, () => {
            
            _animator.SetBool(Dead, false);
            _animator.SetBool(Idle, true);
        }).OnComplete(() => {
            
            dummyHealth = _startHealth;
            _eventArchive.dummyEvents.InvokeOnHealthChanged(dummyHealth, _startHealth);
            _eventArchive.dummyEvents.InvokeOnRespawn();

            DOVirtual.Float(1f, 0f, 1f, dissolve => _material.SetFloat("_DissolveAmount", dissolve))
                .OnComplete(() => {
                    
                    _collider.enabled = true;
                });
        });
    }
}