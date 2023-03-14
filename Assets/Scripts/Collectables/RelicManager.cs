using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicManager : MonoBehaviour
{
    public GameObject[] jumpRelics;
    public GameObject[] runRelics;
    private ResetEvent resetEvent = ResetEvent.Instance;
    void Start()
    {
        resetEvent.AddListener(RespawnRelics);
    }

    private void RespawnRelics() {
        ResetIncompletePlayerStat();
        EnableIncompleteRelics();
    }


    private void EnableIncompleteRelics() {
        if (PlayerStat.jumpCount != 3) {
            foreach (GameObject relic in jumpRelics) {
                relic.SetActive(true);
            }
        }
        if (PlayerStat.runCount != 3) {
            foreach (GameObject relic in runRelics) {
                relic.SetActive(true);
            }
        }
    }

    private void ResetIncompletePlayerStat() {
        if (PlayerStat.jumpCount != 3) {
            PlayerStat.jumpCount = 0;
        }
        if (PlayerStat.runCount != 3) {
            PlayerStat.runCount = 0;
        }
    }
}
