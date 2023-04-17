using UnityEngine;
using UnityEngine.Events;


public class UnpauseEvent : UnityEvent
{
    private static UnpauseEvent _instance;
    public static UnpauseEvent Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UnpauseEvent();
            }

            return _instance;
        }
    }
}