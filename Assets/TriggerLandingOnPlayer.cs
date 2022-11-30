using UnityEngine;

public class TriggerLandingOnPlayer : MonoBehaviour {
    private const string IS_LANDING_ANIMATION_STATE = "IsLanding";
    
    private Animator _animator;
    
    private void Start() {
        _animator = GetComponent<Animator>();
    }

    public void HatLanded() {
        _animator.SetBool(IS_LANDING_ANIMATION_STATE, false);
    }
}
