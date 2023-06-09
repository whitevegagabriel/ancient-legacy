using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetection : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var player = GameObject.Find("Player");
        if (player != null)
        {
            EventManager.TriggerEvent<HitEvent, Vector3>(player.transform.position);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("hit", false);
    }
}
