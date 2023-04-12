using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class NPCChat : MonoBehaviour
{
    private bool _playerInRange;
    private Canvas canvas;
    private GameObject player;
    private GameObject npc;
    public GameObject mainCamera;
    public GameObject introCamera;

    [SerializeField] private TextWriter textWriter;
    private TMP_Text messageText;

    UnityEvent playerInRangeEvent = new UnityEvent();

    bool quitFlag;

    private void Start()
    {
        quitFlag = false;
        canvas = GetComponentInChildren<Canvas>();
        canvas.enabled = false;
        player = GameObject.FindWithTag("Player");
        npc = GameObject.FindWithTag("IntroNPC");
        messageText = GetComponentInChildren<TMP_Text>();
        playerInRangeEvent.AddListener(MyAction);
    }

    private void Update()
    {
        bool playerState = ((player.transform.position - npc.transform.position).sqrMagnitude < 2 * 2);
        playerInRange(playerState);
        if (Input.GetKeyDown("e"))
        {
            quitFlag = false;
            playerInRangeEvent.Invoke();
        }
        if (Input.GetKeyDown("q"))
        {
            quitFlag = true;
        }
    }

    void MyAction()
    {
        textWriter.Addwriter(messageText, "Welcome to AncientLegacy: \n" +
            "1. Player must collect all 3 relics thoughout the first section to unlock the Jump ability. \n" +
            "2. After unlocking Jump ability, Player shall proceed to the next section where Player must collect 3 relics again to unlock the Run ability. \n" +
            "3. After unlocking Run ability, Player shall proceed to the boss room where Player must defeat the boss and collect the final relic to finish the game.", 0.02f);
    }

    private void playerInRange(bool playerInRange)
    {
        if (playerInRange && !quitFlag)
        {
            canvas.enabled = true;
            introCamera.SetActive(true);
            mainCamera.SetActive(false);
        }
        else
        {
            canvas.enabled = false;
            mainCamera.SetActive(true);
            introCamera.SetActive(false);
        }
    }

}

//Welcome to AncientLegacy: 
//1.Player must collect all 3 relics thoughout the first section to unlock the Jump ability. 
//2. After unlocking Jump ability, Player shall proceed to the next section where Player must collect 3 relics again to unlock the Run ability. 
//3. After unlocking Run ability, Player shall proceed to the boss room where Player must defeat the boss and collect the final relic to finish the game. 
