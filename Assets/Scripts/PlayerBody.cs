using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    // Start is called before the first frame update
    
    private void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Hit ground!");
        }
    }
}
