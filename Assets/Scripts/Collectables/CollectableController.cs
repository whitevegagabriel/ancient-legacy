using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController : MonoBehaviour
{
    PlayerController playController;
    public AudioSource collectSound;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            collectSound.Play();
            gameObject.SetActive(false);
        }
    }
}