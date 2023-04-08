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
        resetEvent.AddListener(Reset);
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

    public void Reset() {
        if (PlayerState.JumpCount != 3) {
            for(int i = 0; i < PlayerState.JumpCount; i++) {
                jumpRelicDisplay[i].color = new Color(0,0,0);
            }
            PlayerState.JumpCount = 0;
        }

        if (PlayerState.RunCount != 3) {
            for(int i = 0; i < PlayerState.RunCount; i++) {
                runRelicDisplay[i].color = new Color(0,0,0);
            }
            PlayerState.RunCount = 0;
        }
    }
}
