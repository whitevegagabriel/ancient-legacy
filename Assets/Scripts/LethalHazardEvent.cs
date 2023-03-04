using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LethalHazardEvent : MonoBehaviour
{
    public int dmg = 0;
    void OnTriggerEnter(Collider c)
    {
        CharacterController character = c.gameObject.GetComponent<CharacterController>();
        if (character != null)
        {
            PlayerHealth charHealth = character.gameObject.GetComponent<PlayerHealth>();
            charHealth.DecreaseHealth(dmg);
        }
    }
}
