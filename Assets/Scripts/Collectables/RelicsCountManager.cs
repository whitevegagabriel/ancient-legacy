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
        JumpCountDisplay.text = GetJumpRelicText(PlayerStat.jumpCount);
        RunCountDisplay.text = GetRunRelicText(PlayerStat.runCount);
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
            JumpCountDisplay.text = GetJumpRelicText(PlayerStat.jumpCount);
        }
        
        if (PlayerStat.runCount == 3)
        {
            RunCountDisplay.text = "Press Shift to Run";

        }
        else
        {
            RunCountDisplay.text = GetRunRelicText(PlayerStat.runCount);
        }
    }
    
    private static string GetJumpRelicText(int count)
    {
        return $"Jump Relics: {count} / 3";
    }
    
    private static string GetRunRelicText(int count)
    {
        return $"Run Relics: {count} / 3";
    }
}
