using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    bool isFalling = false;
    float downSpeed = 0;

    void OnCollisionEnter(Collision collision)
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
            downSpeed += Time.deltaTime/5;
            transform.position = new Vector3(transform.position.x,
                transform.position.y - downSpeed,
                transform.position.z);
        }

    }
}
