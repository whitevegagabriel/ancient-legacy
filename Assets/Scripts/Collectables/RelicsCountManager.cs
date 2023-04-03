using System.Collections;
using System.Collections.Generic;
using StateManagement;
using UnityEngine;
using UnityEngine.UI;

public class RelicsCountManager : MonoBehaviour
{
    public Text JumpCountDisplay;
    public Text RunCountDisplay;
    // Start is called before the first frame update
    void Start()
    {
        JumpCountDisplay.text = GetJumpRelicText(PlayerState.JumpCount);
        RunCountDisplay.text = GetRunRelicText(PlayerState.RunCount);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerState.JumpCount == 3)
        {
            JumpCountDisplay.text = "Press Spacebar to Jump";
        }
        else
        {
            JumpCountDisplay.text = GetJumpRelicText(PlayerState.JumpCount);
        }
        
        if (PlayerState.RunCount == 3)
        {
            RunCountDisplay.text = "Press Shift to Run";

        }
        else
        {
            RunCountDisplay.text = GetRunRelicText(PlayerState.RunCount);
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
