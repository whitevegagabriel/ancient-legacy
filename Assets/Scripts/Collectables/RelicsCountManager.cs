using StateManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Collectables
{
    public class RelicsCountManager : MonoBehaviour
    {
        public RawImage[] jumpRelicDisplay;
        public RawImage[] runRelicDisplay;
        private readonly ResetEvent resetEvent = ResetEvent.Instance;

        private void Start()
        {
            resetEvent.AddListener(Reset);
        }

        public void UpdateJumpRelicImage() {
            for(var i = 0; i < PlayerState.JumpCount; i++) {
                jumpRelicDisplay[i].color = new Color(255,255,255);
            }
        }

        public void UpdateRunRelicImage() {
            for(var i = 0; i < PlayerState.RunCount; i++) {
                runRelicDisplay[i].color = new Color(255,255,255);
            }
        }

        public void Reset() {
            if (PlayerState.JumpCount != 3) {
                for(var i = 0; i < PlayerState.JumpCount; i++) {
                    jumpRelicDisplay[i].color = new Color(0,0,0);
                }
                PlayerState.JumpCount = 0;
            }

            if (PlayerState.RunCount != 3) {
                for(var i = 0; i < PlayerState.RunCount; i++) {
                    runRelicDisplay[i].color = new Color(0,0,0);
                }
                PlayerState.RunCount = 0;
            }
        }
    }
}
