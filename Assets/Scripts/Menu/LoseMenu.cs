using UnityEngine;
[RequireComponent(typeof(CanvasGroup))] 
public class LoseMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    void Start()
    {
        Time.timeScale = 1f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;
    }

    public void DisplayOnDeath() {
        Time.timeScale = 0f;
        PlayerController.health = 10;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}