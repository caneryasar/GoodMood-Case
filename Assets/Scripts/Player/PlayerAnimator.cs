using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    
    private static readonly int MoveLockOn = Animator.StringToHash("MoveLockOn");
    private static readonly int MoveFree = Animator.StringToHash("MoveFree");
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int DirectionX = Animator.StringToHash("DirectionX");
    private static readonly int DirectionY = Animator.StringToHash("DirectionY");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Combo = Animator.StringToHash("Combo");

    private string _stateTransition;
    
    private Animator _animator;

    private EventArchive _eventArchive;

    private bool _isLockedOn;
    private bool _isDummyDead = false;
    
    void Start() {
        
        _animator = GetComponent<Animator>();
        
        Subscribe();

        this.UpdateAsObservable().Subscribe(_ => {

            if(_animator.GetAnimatorTransitionInfo(1).IsName("Attack_1 -> Exit") ||
               _animator.GetAnimatorTransitionInfo(1).IsName("Attack_2 -> Exit") ||
               _animator.GetAnimatorTransitionInfo(1).IsName("Attack_3 -> Exit")) {
                
               _animator.SetLayerWeight(1, 0); 
            }
            
            if(_animator.GetCurrentAnimatorStateInfo(1).IsName("Empty")) { _eventArchive.gameplay.InvokeOnAttackComboCount(0); }
            if(_animator.GetAnimatorTransitionInfo(1).IsName("Empty -> Attack_1")) { _eventArchive.gameplay.InvokeOnAttackComboCount(1); }
            if(_animator.GetAnimatorTransitionInfo(1).IsName("Attack_1 -> Attack_2")) { _eventArchive.gameplay.InvokeOnAttackComboCount(2); }
            if(_animator.GetAnimatorTransitionInfo(1).IsName("Attack_2 -> Attack_3")) { _eventArchive.gameplay.InvokeOnAttackComboCount(3); }
            
            if(_animator.GetLayerWeight(1) == 0) { _animator.SetBool(Attack, false);}
        });
        
    }

    private void Subscribe() {

        _eventArchive = FindFirstObjectByType<EventArchive>();

        _eventArchive.playerInputs.OnMove += AnimateMove;
        _eventArchive.playerInputs.OnLockOn += () => _isLockedOn = !_isLockedOn;
        _eventArchive.gameplay.OnAttacking += Attacking;
        _eventArchive.gameplay.OnCombo += CanCombo;
        _eventArchive.dummyEvents.OnDeath += () => _isDummyDead = true;
        _eventArchive.dummyEvents.OnRespawn += () => _isDummyDead = false;
    }


    private void Attacking() {
        
        _animator.SetLayerWeight(1, 1);
        
        _animator.SetBool(Attack, true);
    }
    
    private void CanCombo(bool isComboing) {
        
        _animator.SetBool(Combo, isComboing);

        if(isComboing) { return; }
        
        _animator.SetBool(Attack, false);
        
    }

    private void AnimateMove(Vector2 direction) {

        if(direction != Vector2.zero) {

            if(_isDummyDead) { _isLockedOn = false; }

            if(_isLockedOn) {
                
                _animator.SetBool(MoveFree, false);
                _animator.SetBool(MoveLockOn, true);
            }
            else {
                
                _animator.SetBool(MoveFree, true);
                _animator.SetBool(MoveLockOn, false);
            }
            
            _animator.SetBool(Idle, false);
            
            _animator.SetFloat(DirectionX, direction.x);
            _animator.SetFloat(DirectionY, direction.y);
        }
        else {
            
            _animator.SetBool(Idle, true);
            _animator.SetBool(MoveFree, false);
            _animator.SetBool(MoveLockOn, false);
        }
    }
}