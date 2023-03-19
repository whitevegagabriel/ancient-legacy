using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDetection : StateMachineBehaviour
{

    private float _delay;
    private bool _alreadyDetectedFall;
    private static readonly int Fall = Animator.StringToHash("fall");

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _alreadyDetectedFall = animator.GetBool(Fall) || _alreadyDetectedFall;
        
        if (!(stateInfo.normalizedTime > 1) || _alreadyDetectedFall) return;
        
        if (_delay == 0f) {
            _delay = Time.time + 0.5f;
        }
        else if (_delay < Time.time) {
            animator.SetBool("fall", true);
        }
    }  

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _delay = 0f;
    }         
}
