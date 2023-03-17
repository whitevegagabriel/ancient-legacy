using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallAudioHandler : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        EventManager.TriggerEvent<PlayerLandsEvent, Vector3, PlayerController.airState>(GameObject.Find("Player").transform.position, PlayerController.airState.Fall);
    }
}
