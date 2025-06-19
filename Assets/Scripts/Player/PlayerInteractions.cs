using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour {

    private PlayerData _playerData;
    
    private List<GameObject> _swords;
    private List<Collider> _swordColliders;

    private int _comboCount;

    private EventArchive _eventArchive;
    
    void Start() {
        
        _playerData = GetComponent<PlayerController>().playerData;
        
        _eventArchive = FindFirstObjectByType<EventArchive>();
        _eventArchive.gameplay.OnAttackComboCount += count => _comboCount = count;
        _eventArchive.gameplay.OnDummyHit += ReturnDamage;

        _swords = new List<GameObject>();
        _swordColliders = new List<Collider>();
        
        _swords = GameObject.FindGameObjectsWithTag("Sword").ToList();
        foreach(var sword in _swords) { _swordColliders.Add(sword.gameObject.GetComponentInChildren<Collider>()); }
        
        DisableSwords();
    }

    private void ReturnDamage() {

        var dmg = 0f;

        if(_comboCount < 3) {

            dmg = _comboCount * _playerData.baseDamage;
        }
        else {
            
            dmg = _playerData.finisherDamage;
        }
        _eventArchive.dummyEvents.InvokeOnGetDamage(dmg);
    }

    internal void EnableSwords() {

        if(_comboCount % 2 == 0) {
            
            _swordColliders[1].enabled = true;
            _swordColliders[0].enabled = false;
        }
        else {
            
            _swordColliders[1].enabled = false;
            _swordColliders[0].enabled = true;
        }
        
        // foreach(var swordCollider in _swordColliders) { swordCollider.enabled = true; }
    }

    internal void DisableSwords() {
        
        foreach(var swordCollider in _swordColliders) { swordCollider.enabled = false; }
    }
}