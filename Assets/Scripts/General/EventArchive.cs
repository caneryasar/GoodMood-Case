using System;
using UnityEngine;

public class EventArchive : MonoBehaviour {

    public PlayerInputs playerInputs;
    public Gameplay gameplay;
    
    
    public struct PlayerInputs {

        public event Action<Vector2> OnMove;
        public event Action OnAttack;
        public event Action OnLockOn;

        //

        public void InvokeOnMove(Vector2 move) { OnMove?.Invoke(move); }
        public void InvokeOnAttack() { OnAttack?.Invoke(); }
        public void InvokeOnLockOn() { OnLockOn?.Invoke(); }
    }
    
    public struct Gameplay {
        
        public event Action<bool> OnPlayable;
        public event Action OnDummyHit;
        public event Action OnDummyDeath;
        public event Action<bool> OnCombo;
        public event Action OnAttacking;
        
        //
        
        public void InvokeOnPlayable(bool playable) { OnPlayable?.Invoke(playable); }
        public void InvokeOnDummyHit() { OnDummyHit?.Invoke(); }
        public void InvokeOnDummyDeath() { OnDummyDeath?.Invoke(); }
        public void InvokeOnCombo(bool canCombo) { OnCombo?.Invoke(canCombo); }
        public void InvokeOnAttacking() { OnAttacking?.Invoke(); }
        
    }

     
}