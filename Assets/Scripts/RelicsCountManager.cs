using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RelicsCountManager : MonoBehaviour
{
    public Text JumpCountDisplay;
    public Text RunCountDisplay;
    // Start is called before the first frame update
    void Start()
    {
        JumpCountDisplay.text = "JumpRelics: " + "0 / 3";
        RunCountDisplay.text = "RunRelics: " + "0 / 3";
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerStat.jumpCount == 3)
        {
            JumpCountDisplay.text = "Press Spacebar to Jump";
        }
        else
        {
            JumpCountDisplay.text = "JumpRelics: " + PlayerStat.jumpCount.ToString() + " / 3";
        }
        if (PlayerStat.runCount == 3)
        {
            RunCountDisplay.text = "Press Shift to Run";

        }
        else
        {
            RunCountDisplay.text = "RunRelics: " + PlayerStat.runCount.ToString() + " / 3";
        }
    }
}
