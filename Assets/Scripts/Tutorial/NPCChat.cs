using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Tutorial
{
    public class NPCChat : MonoBehaviour
    {
        private Canvas canvas;
        private GameObject player;
        public GameObject npc;
        public GameObject mainCamera;
        public GameObject introCamera;

        [SerializeField] private TextWriter textWriter;
        private TMP_Text messageText;

        private readonly UnityEvent playerInRangeEvent = new UnityEvent();
        private void Start()
        {
            canvas = GetComponentInChildren<Canvas>();
            canvas.enabled = false;
            player = GameObject.FindWithTag("Player");
            messageText = GetComponentInChildren<TMP_Text>();
            playerInRangeEvent.AddListener(MyAction);
            mainCamera.SetActive(true);
            introCamera.SetActive(false);

        }

        private void Update()
        {
            var playerInRange = (player.transform.position - npc.transform.position).sqrMagnitude < 3 * 2;
            
            canvas.enabled = playerInRange;

            if (playerInRange && Input.GetKeyDown("e") && npc.tag == introCamera.tag)
            {
                playerInRangeEvent.Invoke();
                player.SetActive(false);
                mainCamera.SetActive(false);
                introCamera.SetActive(true);
            }
            if (playerInRange && Input.GetKeyDown("q") && npc.tag == introCamera.tag)
            {
                textWriter.Reset();
                messageText.text = "";
                player.SetActive(true);
                introCamera.SetActive(false);
                mainCamera.SetActive(true);
            }
        }

        private void MyAction()
        {
            switch (npc.tag)
            {
                case "IntroNPC":
                    textWriter.Addwriter(messageText, "Welcome to AncientLegacy! \n" +
                                                      "1. Wait for the moving platform \n" +
                                                      "2. Walk on the platform to cross the ravine", 0.02f);
                    break;
                case "LevelOneNPC":
                    textWriter.Addwriter(messageText,
                        "1. Look around and search for relics \n" +
                        "2. Collect three Relics to enable abilities", 0.02f);
                    break;
                case "OrbNPC":
                    textWriter.Addwriter(messageText,
                        "1. Watch out for enemies \n" +
                        "2. Attack them or block their attacks", 0.02f);
                    break;
                case "CollapseNPC":
                    textWriter.Addwriter(messageText,
                        "1. Watch out for collapsing floors \n" +
                        "2. Don't forget to use your new ability", 0.02f);
                    break;
            }
        }
    }
}

