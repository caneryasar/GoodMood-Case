using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour {

    private PlayerData _playerData;
    
    private List<GameObject> _swords;
    private List<Collider> _swordColliders;
    private ParticleSystem[] _leftSwordParticles;
    private ParticleSystem[] _rightSwordParticles;

    private int _comboCount;

    private EventArchive _eventArchive;
    
    void Start() {
        
        _playerData = GetComponent<PlayerController>().playerData;
        
        Subscribe();

        _swords = new List<GameObject>();
        _swordColliders = new List<Collider>();
        
        _swords = GameObject.FindGameObjectsWithTag("Sword").ToList();
        foreach(var sword in _swords) { _swordColliders.Add(sword.gameObject.GetComponentInChildren<Collider>()); }
        
        _leftSwordParticles = _swords[0].GetComponentsInChildren<ParticleSystem>();
        _rightSwordParticles = _swords[1].GetComponentsInChildren<ParticleSystem>();
        
        foreach(var leftSwordParticle in _leftSwordParticles) { leftSwordParticle.Stop(); }
        foreach(var rightSwordParticle in _rightSwordParticles) { rightSwordParticle.Stop(); }
        
        DisableSwords();
    }

    private void Subscribe() {
        
        _eventArchive = FindFirstObjectByType<EventArchive>();
        _eventArchive.gameplay.OnAttackComboCount += count => _comboCount = count;
        _eventArchive.gameplay.OnDummyHit += ReturnDamage;
    }

    private void ReturnDamage() {

        var dmg = 0f;

        if(_comboCount < 3) { dmg = _comboCount * _playerData.baseDamage; }
        else { dmg = _playerData.finisherDamage; }
        _eventArchive.dummyEvents.InvokeOnGetDamage(dmg);
    }

    internal void EnableSwords() {

        if(_comboCount % 2 == 0) {
            
            _swordColliders[1].enabled = true;
            _swordColliders[0].enabled = false;
        
            foreach(var rightSwordParticle in _rightSwordParticles) { rightSwordParticle.Play(); }
        }
        else {
            
            _swordColliders[1].enabled = false;
            _swordColliders[0].enabled = true;
        
            foreach(var leftSwordParticle in _leftSwordParticles) { leftSwordParticle.Play(); }
        }
    }

    internal void DisableSwords() {
        
        foreach(var swordCollider in _swordColliders) { swordCollider.enabled = false; }
        
        foreach(var leftSwordParticle in _leftSwordParticles) { leftSwordParticle.Stop(); }
        foreach(var rightSwordParticle in _rightSwordParticles) { rightSwordParticle.Stop(); }
    }
}