using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    private bool isFalling = false;
    private float downSpeed = 0f;
    private Vector3 originalPosition;
    private ResetEvent resetEvent = ResetEvent.Instance;

    void Start() {
        originalPosition = transform.localPosition;
        resetEvent.AddListener(ResetTile);
    }

    void OnTriggerEnter(Collider collision)
    {
        //can add a check first
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(MakeTileFall());
        }
    }

    IEnumerator MakeTileFall()
    {
        yield return new WaitForSeconds(.15f);
        isFalling = true;
    }

    void Update()
    {   
        if (isFalling)
        {
            downSpeed += Time.deltaTime/20;
            transform.position = new Vector3(transform.position.x,
                transform.position.y - downSpeed,
                transform.position.z);
        }

    }

    public void ResetTile() {
        this.isFalling = false;
        this.downSpeed = 0f;
        this.transform.localPosition = originalPosition;
    }
}