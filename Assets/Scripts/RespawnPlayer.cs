using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    private Vector3 _respawnPoint;
    private ResetEvent resetEvent = ResetEvent.Instance;
    void Start() {
        _respawnPoint = new Vector3(20.72f, 7.1f, -8.6f);
        resetEvent.AddListener(Respawn);
    }
    void Update() {
        if (transform.position.y < -5) {
            resetEvent.Invoke();
        }
    }

    public void Respawn() {
        CharacterController cc = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
        cc.enabled = false;
        transform.position = _respawnPoint;
        cc.enabled = true;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("CheckPoint")) {
            _respawnPoint = other.transform.localPosition;
        }
    }
}