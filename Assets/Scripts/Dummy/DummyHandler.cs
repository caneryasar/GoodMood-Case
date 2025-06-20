using System;
using System.Collections.Generic;
using DG.Tweening;
using Microsoft.Win32.SafeHandles;
using UniRx;
using UniRx.Triggers;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.VFX;

public class DummyHandler : MonoBehaviour {

    public AudioData audioData;
    
    [SerializeField] private float dummyHealth = 100;

    private float _startHealth;
    private int _comboCount;
    
    private CapsuleCollider _collider;
    private Animator _animator;
    private CinemachineImpulseSource _impulseSource;
    private SkinnedMeshRenderer[] _renderers;
    private List<Material> _materials;
    private VisualEffect _impact;
    private AudioSource _audioSource;
    private Rigidbody _rigidbody;
    
    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Dead = Animator.StringToHash("Dead");
    
    private EventArchive _eventArchive;

    void Start() {
        
        _startHealth = dummyHealth;

        _materials = new List<Material>();

        _collider = GetComponentInChildren<CapsuleCollider>();
        _animator = GetComponent<Animator>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach(var skinnedMeshRenderer in _renderers) { _materials.Add(skinnedMeshRenderer.material); }
        _impact = GetComponentInChildren<VisualEffect>(true);
        _impact.gameObject.SetActive(false);
        _audioSource = GetComponentInChildren<AudioSource>();
        _rigidbody = GetComponent<Rigidbody>();
        
        Subscribe();
    }

    private void Subscribe() {
        
        _eventArchive = FindFirstObjectByType<EventArchive>();

        _eventArchive.dummyEvents.OnGetDamage += ReceiveDamage;
        _eventArchive.gameplay.OnAttackComboCount += count => _comboCount = count;
    }
    
    private void OnTriggerEnter(Collider other) {

        if(dummyHealth <= 0) { return; }

        if(!other.CompareTag("Sword")) { return; }
        
        _eventArchive.gameplay.InvokeOnDummyHit();
        _impulseSource.GenerateImpulseWithForce(0.5f * _comboCount);

        var impactSpawn = _collider.ClosestPointOnBounds(other.transform.position);
            
        _impact.transform.position = impactSpawn;
        _impact.gameObject.SetActive(true);
        _impact.Play();
            
        DOVirtual.DelayedCall(1f, () => _impact.gameObject.SetActive(false));
    }

    private void ReceiveDamage(float dmg) {

        if(dmg > dummyHealth) {

            dummyHealth = 0;
            
            DeathAnimation();
            _eventArchive.dummyEvents.InvokeOnDeath();
            
            _audioSource.clip = audioData.dummyDeath;
            _audioSource.volume = .5f;
        }
        else {
            
            dummyHealth -= dmg;
            HitAnimation();
            
            _audioSource.clip = audioData.hitSounds[_comboCount - 1];
            _audioSource.volume = .75f;
        }

        _audioSource.Play();
        
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

        _rigidbody.isKinematic = true;
        
        _animator.SetBool(Idle, false);
        _animator.SetBool(Dead, true);

        DOVirtual.Float(0f, 1f, 4f, dissolve => {
            
            foreach(var material in _materials) { material.SetFloat("_DissolveAmount", dissolve); }
        });
        
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

            DOVirtual.Float(1f, 0f, 1f, dissolve => {
            
                    foreach(var material in _materials) { material.SetFloat("_DissolveAmount", dissolve); }
                }).OnComplete(() => { _rigidbody.isKinematic = false; });
        });
    }
}