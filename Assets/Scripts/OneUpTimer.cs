using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneUpTimer : MonoBehaviour
{
    private float _startTime;
    private readonly DisplayOneUpEvent _displayOneUpEvent = DisplayOneUpEvent.Instance;
    private CanvasGroup _canvasGroup;
    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0f;
        _displayOneUpEvent.AddListener(DisplayOneUp);
    }

    private void DisplayOneUp()
    {
        StartCoroutine(DisplayOneUpTimer());
    }
    
    private IEnumerator DisplayOneUpTimer()
    {
        _canvasGroup.alpha = 1f;
        yield return new WaitForSeconds(2f);
        _canvasGroup.alpha = 0f;
    }
}
