using UnityEngine;

namespace SceneManagement
{
    public class GameQuitter : MonoBehaviour
    {
        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
         System.Diagnostics.Process.GetCurrentProcess().Kill();
#endif
        }
    }
}
