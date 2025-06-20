using UnityEngine;

public class PlayerAudioHandler : MonoBehaviour {
    
    public AudioData audioData;
    
    private AudioSource _audioSource;

    private bool _isLockOn = false;
    private bool _isDummyDead = false;
    
    private EventArchive _eventArchive;
    
    
    void Start() {
        
        _audioSource = GetComponentInChildren<AudioSource>();

        Subscribe();
    }

    private void Subscribe() {

        _eventArchive = FindFirstObjectByType<EventArchive>();

        _eventArchive.playerInputs.OnMove += PlayWalkSound;
        _eventArchive.playerInputs.OnLockOn += SetLockOn;
        _eventArchive.gameplay.OnAttackComboCount += PlayAttackSounds;
        _eventArchive.dummyEvents.OnDeath += () => _isDummyDead = true;
        _eventArchive.dummyEvents.OnRespawn += () => _isDummyDead = false;
    }

    private void SetLockOn() {
        
        if(_isDummyDead) { return; }

        _isLockOn = !_isLockOn;
    }

    private void PlayWalkSound(Vector2 direction) {

        if(direction == Vector2.zero) {
            
            _audioSource.Stop();
            _audioSource.loop = false;
            _audioSource.resource = null;
            
            return;
        }

        _audioSource.loop = true;
        _audioSource.resource = audioData.walkSound;
        _audioSource.Play();
    }

    private void PlayAttackSounds(int comboCount) {
        
        if(comboCount == 0) { return; }
        
        _audioSource.loop = false;
        // _audioSource.resource = audioData.attackSounds[comboCount - 1];
        _audioSource.PlayOneShot(audioData.attackSounds[comboCount - 1]);
    }
}