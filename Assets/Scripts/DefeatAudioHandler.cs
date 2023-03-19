using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatAudioHandler : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       EventManager.TriggerEvent<PlayerDeathEvent, Vector3>(GameObject.Find("Player").transform.position);
    }
}
