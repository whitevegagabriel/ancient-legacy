using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeathAudioHandler : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        EventManager.TriggerEvent<SkeletonDeathEvent1, Vector3>(animator.gameObject.transform.position);
    }
}