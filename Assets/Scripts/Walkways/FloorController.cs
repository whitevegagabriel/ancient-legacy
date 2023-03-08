using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    bool isFalling = false;
    float downSpeed = 0;

    void OnTriggerEnter(Collider collision)
    {
        //can add a check first
        if (collision.gameObject.tag == "Player")
        {
            isFalling = true;
        }
    }

    void Update()
    {
        if (isFalling)
        {
            downSpeed += Time.deltaTime/10;
            transform.parent.position = new Vector3(transform.parent.position.x,
                transform.parent.position.y - downSpeed,
                transform.parent.position.z);
        }

    }
}
