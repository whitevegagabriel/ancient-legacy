using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LethalHazardEvent : MonoBehaviour
{
    void OnTriggerEnter(Collider c)
    {
        CharacterController character = c.gameObject.GetComponent<CharacterController>();
        if (character != null)
        {
            SceneManager.LoadScene("Main");
        }
    }
}
