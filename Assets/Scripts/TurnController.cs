using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    public Transform GameCamera;
 
    private Quaternion CharacterRotation;

    private PlayerController playerController;

    void Start () {
        playerController = GetComponent<PlayerController>();
    }

    void Update ()
    {  
        if (!playerController.isDefeated) {
            CharacterRotation = GameCamera.transform.rotation;
            CharacterRotation.x = 0;
            CharacterRotation.z = 0;
            transform.rotation = CharacterRotation;
        }
    }

}
