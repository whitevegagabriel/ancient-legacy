using StateManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Collectables
{
    public class RelicsCountManager : MonoBehaviour
    {
        public RawImage[] jumpRelicDisplay;
        public RawImage[] runRelicDisplay;

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
    }
}
