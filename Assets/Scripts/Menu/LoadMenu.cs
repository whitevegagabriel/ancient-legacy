using SceneManagement;
using UnityEngine;

namespace Menu
{
    public class LoadMenu : MonoBehaviour
    {
        public void StartMenu()
        {
            SceneLoader.LoadScene(SceneName.StartMenu);
        }
    }
}
