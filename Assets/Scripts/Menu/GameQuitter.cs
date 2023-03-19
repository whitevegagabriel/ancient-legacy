using UnityEngine;

namespace Menu
{
    public class GameQuitter : MonoBehaviour
    {
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
        }
    }
}
