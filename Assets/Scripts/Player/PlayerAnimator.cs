using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    
    private static readonly int MoveLockOn = Animator.StringToHash("MoveLockOn");
    private static readonly int MoveFree = Animator.StringToHash("MoveFree");
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int DirectionX = Animator.StringToHash("DirectionX");
    private static readonly int DirectionY = Animator.StringToHash("DirectionY");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Combo = Animator.StringToHash("Combo");

    private Animator _animator;

    private EventArchive _eventArchive;

    private bool _isLockedOn;
    
    void Start() {
        
        _animator = GetComponent<Animator>();
        
        Subscribe();
    }
    void Update() {
        
    }


    private void Subscribe() {

        _eventArchive = FindFirstObjectByType<EventArchive>();

        _eventArchive.playerInputs.OnMove += AnimateMove;
        _eventArchive.playerInputs.OnLockOn += () => _isLockedOn = !_isLockedOn;
        _eventArchive.gameplay.OnAttacking += Attacking;
        _eventArchive.gameplay.OnCombo += CanCombo;
    }


    private void Attacking() {
        
        _animator.SetLayerWeight(1, 1);
        
        _animator.SetBool(Attack, true);
    }
    
    private void CanCombo(bool isComboing) {
        
        _animator.SetBool(Combo, isComboing);

        if(isComboing) { return; }

        if(!(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1) || _animator.IsInTransition(0)) { return; }
        
        _animator.SetLayerWeight(1, 0);
        _animator.SetBool(Attack, isComboing);
    }

    private void AnimateMove(Vector2 direction) {

        if(direction != Vector2.zero) {

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