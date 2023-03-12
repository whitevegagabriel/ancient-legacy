using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LethalHazardEvent : MonoBehaviour
{
    public int dmg = 0;
    private int time = 0;


    void OnTriggerEnter(Collider c)
    {
        if (c.attachedRigidbody != null)
        {
            WeaponController pc = c.attachedRigidbody.gameObject.GetComponent<WeaponController>();
            if (pc != null) {
                if (time == 0)
                {
                    GameObject character = GameObject.FindGameObjectWithTag("Player");
                    var playerTargetable = character.GetComponent<Targetable>();
                    playerTargetable.OnHit(dmg);
                    time = 100;
                }
            }
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
