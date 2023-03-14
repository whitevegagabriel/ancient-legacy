using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicManager : MonoBehaviour
{
    public GameObject jumpRelicPrefab;
    public GameObject runRelicPrefab;
    public GameObject[] jumpRelics;
    public GameObject[] runRelics;
    private Vector3[] runRelicLocations;
    private Vector3[] jumpRelicLocations;
    private ResetEvent resetEvent = ResetEvent.Instance;

    void Start()
    {
        jumpRelicLocations = new Vector3[3];
        runRelicLocations = new Vector3[3];
        for (int i = 0; i < 3; i++) {
            jumpRelicLocations[i] = jumpRelics[i].transform.position;
            runRelicLocations[i] = runRelics[i].transform.position;
        }
        resetEvent.AddListener(RespawnRelics);
    }

    private void RespawnRelics() {
        DestroyCurrentRelics();
        ResetIncompletePlayerStat();
        if (PlayerStat.jumpCount == 0) {
            foreach (Vector3 location in jumpRelicLocations) {
                Instantiate(jumpRelicPrefab, location, Quaternion.identity);

            }
        }

        if (PlayerStat.runCount == 0) {
            foreach (Vector3 location in runRelicLocations) {
                Instantiate(runRelicPrefab, location, Quaternion.identity);
            }
        }
    }


    private void DestroyCurrentRelics() {
        GameObject[] jumpRelics = GameObject.FindGameObjectsWithTag("JumpRelics");
        GameObject[] runRelics = GameObject.FindGameObjectsWithTag("RunRelics");

        foreach (GameObject relic in jumpRelics) {
            Destroy(relic);
        }
        foreach (GameObject relic in runRelics) {
            Destroy(relic);
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
