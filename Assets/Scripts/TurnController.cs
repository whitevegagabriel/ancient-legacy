using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    public Transform GameCamera;
 
    private Quaternion CharacterRotation;

    void Update ()
    {
        CharacterRotation = GameCamera.transform.rotation;
        CharacterRotation.x = 0;
        CharacterRotation.z = 0;
        transform.rotation = CharacterRotation;
    }

}
