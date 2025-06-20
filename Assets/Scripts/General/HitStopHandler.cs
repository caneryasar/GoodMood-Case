using System.Collections;
using DG.Tweening;
using UnityEngine;

public class HitStopHandler : MonoBehaviour {

    [SerializeField] private float hitStopTime = .5f;

    private float _defaultTimeScale;
    
    private EventArchive _eventArchive;
    
    
    void Start() {

        _defaultTimeScale = Time.timeScale;
        
        _eventArchive = FindFirstObjectByType<EventArchive>();
        _eventArchive.gameplay.OnDummyHit += StopTime;
    }

    private void StopTime() {

        Time.timeScale = 0f;

        DOVirtual.DelayedCall(hitStopTime, () => { Time.timeScale = _defaultTimeScale; });
    }

}