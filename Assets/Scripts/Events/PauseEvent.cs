using UnityEngine;
using UnityEngine.Events;


public class PauseEvent : UnityEvent
{
    private static PauseEvent _instance;
    public static PauseEvent Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PauseEvent();
            }

            return _instance;
        }
    }
}