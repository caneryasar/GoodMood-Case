using UnityEngine;

public class UIHandler : MonoBehaviour {

    private EventArchive _eventArchive;
    
    void Start() {

        _eventArchive = FindFirstObjectByType<EventArchive>();

    }

    public void StartGame() {
        
        _eventArchive.gameplay.InvokeOnPlayable(true);
        gameObject.SetActive(false);
    }
}