using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadBoss : MonoBehaviour
{
    
    void OnTriggerEnter(Collider c)
    {
        if (c.attachedRigidbody != null)
        {
            WeaponController pc = c.attachedRigidbody.gameObject.GetComponent<WeaponController>();
            if (pc != null)
            {
                SceneManager.LoadScene("LevelOneBossScene");
            }
        }
    }
}
