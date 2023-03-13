using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathDetection : StateMachineBehaviour
{
    float afterAnimation = 0f;
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (afterAnimation == 0f) {
            afterAnimation = stateInfo.length + Time.time;
        }

        if (Time.time > afterAnimation) {
           GameObject.FindGameObjectWithTag("LoseMenu").GetComponent<LoseMenu>().DisplayOnDeath();
        }
    }
}
