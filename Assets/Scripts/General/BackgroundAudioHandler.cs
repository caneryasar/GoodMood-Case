using UnityEngine;

public class BackgroundAudioHandler : MonoBehaviour {
    
    public AudioData audioData;
    
    private AudioSource _audioSource;
    
    void Start() {
        
        if(Camera.main != null) { _audioSource = Camera.main.GetComponent<AudioSource>(); }

        _audioSource.volume = .1f;
        _audioSource.loop = true;;
        _audioSource.clip = audioData.bgMusic;
        _audioSource.Play();
    }

}