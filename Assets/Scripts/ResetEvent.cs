using UnityEngine;
using UnityEngine.Events;


public class ResetEvent : UnityEvent
{
    private static ResetEvent _instance;
    public static ResetEvent Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ResetEvent();
            }
            return _instance;
        }
    }
}