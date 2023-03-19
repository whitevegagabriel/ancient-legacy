using UnityEngine;

namespace Menu
{
    [RequireComponent(typeof(CanvasGroup))] 
    public class VictoryMenu : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        void Start()
        {
            Time.timeScale = 1f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0f;
        }

        public void DisplayOnCollection() {
            PlayerController.health = 10;
            Time.timeScale = 0f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}