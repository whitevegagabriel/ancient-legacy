using System.Collections;
using System.Collections.Generic;
using StateManagement;
using UnityEngine;
using UnityEngine.UI;

public class RelicsCountManager : MonoBehaviour
{
    public Text JumpCountDisplay;
    public Text RunCountDisplay;
    public RawImage[] jumpRelicDisplay;
    public RawImage[] runRelicDisplay;
    private ResetEvent resetEvent = ResetEvent.Instance;
    // Start is called before the first frame update
    void Start()
    {
        resetEvent.AddListener(resetImages);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void updateJumpRelicImage() {
        for(int i = 0; i < PlayerState.JumpCount; i++) {
            jumpRelicDisplay[i].color = new Color(255,255,255);
        }
    }

    public void updateRunRelicImage() {
        for(int i = 0; i < PlayerState.RunCount; i++) {
            runRelicDisplay[i].color = new Color(255,255,255);
        }
    }

    public void resetImages() {
        for(int i = 0; i < (jumpRelicDisplay.Length - PlayerState.JumpCount); i++) {
            jumpRelicDisplay[i].color = new Color(0,0,0);
        }

        for(int i = 0; i < (runRelicDisplay.Length - PlayerState.RunCount); i++) {
            runRelicDisplay[i].color = new Color(0,0,0);
        }
    }
}
