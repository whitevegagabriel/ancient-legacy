using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCChat : MonoBehaviour
{
    public float distance;
    public GameObject introText;
    public GameObject player;

    // Start is called before the first frame update
    void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= 3)
        {
            introText.GetComponent<Text>().text = "Test";
        }
    }

}
