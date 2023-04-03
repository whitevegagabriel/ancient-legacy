using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
    public class LoadBoss : MonoBehaviour
    {
        public void OnTriggerEnter(Collider c)
        {
            if (c.CompareTag("Player"))
            {
                SceneLoader.LoadScene(SceneName.LevelOneBossScene);
            }
        }
    }
}
