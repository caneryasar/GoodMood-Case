using System.Linq;
using UnityEngine;

public class PlayerAudioHandler : MonoBehaviour {
    
    public AudioData audioData;
    
    private AudioSource _walkAudioSource;
    private AudioSource _attackAudioSource;

    private bool _isLockOn = false;
    private bool _isDummyDead = false;
    private int _comboCount;
    
    private EventArchive _eventArchive;
    
    
    void Start() {

        var audioSources = GetComponentsInChildren<AudioSource>();
        
        foreach(var audioSource in audioSources) {

            if(audioSource.gameObject.name.Contains("Attack")) { _attackAudioSource = audioSource; }
            else { _walkAudioSource = audioSource; }
        }

        Subscribe();
    }

    private void Subscribe() {

        _eventArchive = FindFirstObjectByType<EventArchive>();

        _eventArchive.playerInputs.OnMove += PlayWalkSound;
        _eventArchive.playerInputs.OnLockOn += SetLockOn;
        _eventArchive.gameplay.OnAttackComboCount += count => _comboCount = count;
        _eventArchive.dummyEvents.OnDeath += () => _isDummyDead = true;
        _eventArchive.dummyEvents.OnRespawn += () => _isDummyDead = false;
    }

    private void SetLockOn() {
        
        if(_isDummyDead) { return; }

        _isLockOn = !_isLockOn;
    }

    private void PlayWalkSound(Vector2 direction) {

        if(direction == Vector2.zero) {
            
            _walkAudioSource.Stop();
            _walkAudioSource.loop = false;
            _walkAudioSource.resource = null;
            _walkAudioSource.pitch = 1f;
            _walkAudioSource.volume = 1f;
            return;
        }
        
        _walkAudioSource.loop = true;
        _walkAudioSource.resource = audioData.walkSound;
        _walkAudioSource.volume = .35f;
        _walkAudioSource.pitch = _isLockOn ? 1.5f : 1.6f;

        if(_walkAudioSource.isPlaying && _walkAudioSource.clip == audioData.walkSound){ return; }
        _walkAudioSource.Play();
    }

    private void PlayAttackSounds() {
        
        if(_comboCount == 0) { return; }
        
        _attackAudioSource.loop = false;
        _attackAudioSource.volume = .75f;
        _attackAudioSource.clip = audioData.attackSounds[_comboCount - 1];
        _attackAudioSource.Play();
    }
}