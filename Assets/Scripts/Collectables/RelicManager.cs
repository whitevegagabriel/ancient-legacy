using StateManagement;
using UnityEngine;

namespace Collectables
{
    public class RelicManager : MonoBehaviour
    {
        public GameObject[] jumpRelics;
        public GameObject[] runRelics;
        private readonly ResetEvent resetEvent = ResetEvent.Instance;

        private void Start()
        {
            resetEvent.AddListener(RespawnRelics);
        }

        private void RespawnRelics() {
            EnableIncompleteRelics();
        }


        private void EnableIncompleteRelics() {
            if (PlayerState.JumpCount != 3) {
                foreach (var relic in jumpRelics) {
                    relic.SetActive(true);
                }
            }
            if (PlayerState.RunCount != 3) {
                foreach (var relic in runRelics) {
                    relic.SetActive(true);
                }
            }
        }
    }
}
