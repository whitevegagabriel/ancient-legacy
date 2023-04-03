using StateManagement;
using UnityEngine;

namespace SceneManagement
{
    public class GameStarter : MonoBehaviour
    {

        void OnTriggerEnter(Collider c)
        {
            if (c.CompareTag("Player"))
            {
                SceneLoader.LoadScene(SceneName.LevelOneScene);
            }
        }
    }
}
