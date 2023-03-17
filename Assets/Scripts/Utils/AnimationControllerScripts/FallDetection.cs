using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDetection : StateMachineBehaviour
{

    float delay = 0f;

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (stateInfo.normalizedTime > 1) {
            if (delay == 0f) {
                delay = Time.time + 0.7f;
            }
            else {
                if (Time.time > delay) {   
                    animator.SetBool("fall", true);
                }
            }
        }
    }  

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        delay = 0f;
    }         
}
