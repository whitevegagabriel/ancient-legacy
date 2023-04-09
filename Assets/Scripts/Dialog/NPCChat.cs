using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class NPCChat : MonoBehaviour
{
    private bool _playerInRange;
    private Canvas canvas;
    private GameObject player;
    private GameObject npc;

    private void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
        canvas.enabled = false;
        player = GameObject.FindWithTag("Player");
        npc = GameObject.FindWithTag("IntroNPC");
    }

    private void Update()
    {
        playerInRange((player.transform.position - npc.transform.position).sqrMagnitude < 3 * 3);
       
    }

    private void playerInRange(bool playerInRange)
    {
        if (playerInRange)
        {
            canvas.enabled = true;
        }
        else
        {
            canvas.enabled = false;
        }
    }

}
