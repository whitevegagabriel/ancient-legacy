using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDetection : StateMachineBehaviour
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (stateInfo.normalizedTime > 1) {
            animator.SetBool("fall", true);
        }
    }
}
