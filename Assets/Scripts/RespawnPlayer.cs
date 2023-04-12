using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    private Vector3 _respawnPoint;
    private readonly ResetEvent resetEvent = ResetEvent.Instance;
    private readonly SaveEvent saveEvent = SaveEvent.Instance;
    void Start() {
        _respawnPoint = transform.position;
        resetEvent.AddListener(Respawn);
    }
    void Update() {
        if (transform.position.y < -5) {
            resetEvent.Invoke();
        }
    }

    private void Respawn() {
        var cc = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        cc.enabled = false;
        transform.position = _respawnPoint;
        cc.enabled = true;
    }

    private void OnTriggerEnter(Collider other) {
        var checkpoint = other.GetComponent<ICheckpoint>();
        var checkpointData = checkpoint?.GetCheckPointData();
        if (checkpointData == null) return;
        
        _respawnPoint = checkpointData.respawnPoint;
        saveEvent.Invoke();
    }
}