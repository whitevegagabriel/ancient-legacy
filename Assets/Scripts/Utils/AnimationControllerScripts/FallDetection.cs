using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDetection : StateMachineBehaviour
{

    private float _delay;
    private static readonly int Fall = Animator.StringToHash("fall");

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!(stateInfo.normalizedTime > 1) || animator.GetBool(Fall)) return;
        
        if (_delay == 0f) {
            _delay = Time.time + 0.5f;
        }
        else if (_delay < Time.time) {
            animator.SetBool(Fall, true);
        }
    }  

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _delay = 0f;
    }         
}
