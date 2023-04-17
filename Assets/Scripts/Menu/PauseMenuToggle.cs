using UnityEngine;

namespace Menu
{
    [RequireComponent(typeof(CanvasGroup))] 
    public class PauseMenuToggle : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        void Awake()
        {
            // canvasGroup = GetComponent<CanvasGroup>();
            // if (canvasGroup == null) {
            //     Debug.LogError("Canvas Group not found");
            // }
            Time.timeScale = 1f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0f;

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape)) {
                if (canvasGroup.interactable) {
                    Time.timeScale = 1f;
                    canvasGroup.interactable = false;
                    canvasGroup.blocksRaycasts = false;
                    canvasGroup.alpha = 0f;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    
                    UnpauseEvent.Instance.Invoke();
                } else {
                    Time.timeScale = 0f;
                    canvasGroup.interactable = true;
                    canvasGroup.blocksRaycasts = true;
                    canvasGroup.alpha = 1f;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;

                    PauseEvent.Instance.Invoke();
                }
            } 
        }
    }
}
