using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour {

    private PlayerData _playerData;
    
    private List<GameObject> _swords;
    private GameObject[] _swordArray;
    private List<Collider> _swordColliders;
    private ParticleSystem[] _leftSwordParticles;
    private ParticleSystem[] _rightSwordParticles;

    private int _comboCount;

    private EventArchive _eventArchive;
    
    void Start() {
        
        _playerData = GetComponent<PlayerController>().playerData;
        
        Subscribe();

        _swords = new List<GameObject>(new GameObject[2]);
        _swordColliders = new List<Collider>(new Collider[2]);
        
        var swords = GameObject.FindGameObjectsWithTag("Sword").ToList();
        
        foreach(var sword in swords) {
            
            if(sword.name.Contains("Left")) {
                
                _swords[0] = sword;
                _swordColliders[0] = _swords[0].gameObject.GetComponentInChildren<Collider>();
                _leftSwordParticles = _swords[0].GetComponentsInChildren<ParticleSystem>();
            }
            else {

                _swords[1] = sword;
                _swordColliders[1] = _swords[1].gameObject.GetComponentInChildren<Collider>();
                _rightSwordParticles = _swords[1].GetComponentsInChildren<ParticleSystem>();
            }
            
        }
        
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