using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LethalHazardEvent : MonoBehaviour
{
    public int dmg = 0;
    private int time = 0;

    void OnTriggerEnter(Collider c)
    {
        if (time == 0)
        {
            GameObject character = GameObject.FindGameObjectWithTag("Player");
            PlayerHealth charHealth = character.GetComponent<PlayerHealth>();
            charHealth.DecreaseHealth(dmg);
            time = 200;
        }
    }

    void FixedUpdate()
    {
        if (time > 0)
        {
            time = time - 1;
        }
    }
}
