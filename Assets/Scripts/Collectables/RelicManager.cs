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
}
