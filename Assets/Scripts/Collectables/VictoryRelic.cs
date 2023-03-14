using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryRelic : MonoBehaviour
{
    PlayerController playController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject.FindGameObjectWithTag("VictoryMenu").GetComponent<VictoryMenu>().DisplayOnCollection();
            Destroy(gameObject);
        }
    }
}