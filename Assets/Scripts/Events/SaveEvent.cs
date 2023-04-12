using UnityEngine;
using UnityEngine.Events;


public class SaveEvent : UnityEvent
{
    private static SaveEvent _instance;
    public static SaveEvent Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SaveEvent();
            }

            return _instance;
        }
    }
}