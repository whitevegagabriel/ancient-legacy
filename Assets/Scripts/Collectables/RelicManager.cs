using System.Collections;
using System.Collections.Generic;
using StateManagement;
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
        if (PlayerState.JumpCount != 3) {
            foreach (GameObject relic in jumpRelics) {
                relic.SetActive(true);
            }
        }
        if (PlayerState.RunCount != 3) {
            foreach (GameObject relic in runRelics) {
                relic.SetActive(true);
            }
        }
    }

    private void ResetIncompletePlayerStat() {
        if (PlayerState.JumpCount != 3) {
            PlayerState.JumpCount = 0;
        }
        if (PlayerState.RunCount != 3) {
            PlayerState.RunCount = 0;
        }
    }
}
