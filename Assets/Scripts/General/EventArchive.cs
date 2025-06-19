using System;
using UnityEngine;

public class EventArchive : MonoBehaviour {

    public PlayerInputs playerInputs;
    public Gameplay gameplay;
    public DummyEvents dummyEvents;
    
    
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
        public event Action OnEnableSwordCollider;
        public event Action OnDisableSwordCollider;
        public event Action<int> OnAttackComboCount; 
        
        //
        
        public void InvokeOnPlayable(bool playable) { OnPlayable?.Invoke(playable); }
        public void InvokeOnDummyHit() { OnDummyHit?.Invoke(); }
        public void InvokeOnDummyDeath() { OnDummyDeath?.Invoke(); }
        public void InvokeOnCombo(bool canCombo) { OnCombo?.Invoke(canCombo); }
        public void InvokeOnAttacking() { OnAttacking?.Invoke(); }
        public void InvokeOnEnableSwordCollider() { OnEnableSwordCollider?.Invoke(); }
        public void InvokeOnDisableSwordCollider() { OnDisableSwordCollider?.Invoke(); }
        public void InvokeOnAttackComboCount(int count) { OnAttackComboCount?.Invoke(count); }
    }
    
    public struct DummyEvents {

        public event Action<float> OnGetDamage;
        public event Action<float, float> OnHealthChanged;
        public event Action OnDeath;
        public event Action OnRespawn;
        
        
        public void InvokeOnGetDamage(float damage) { OnGetDamage?.Invoke(damage); }
        public void InvokeOnHealthChanged(float remaining, float baseHealth) { OnHealthChanged?.Invoke(remaining, baseHealth); }
        public void InvokeOnDeath() { OnDeath?.Invoke(); }
        public void InvokeOnRespawn() { OnRespawn?.Invoke(); }
    }

     
}