using UnityEngine;
using UnityEngine.Events;


public class DisplayOneUpEvent : UnityEvent
{
    private static DisplayOneUpEvent _instance;
    public static DisplayOneUpEvent Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DisplayOneUpEvent();
            }

            return _instance;
        }
    }
}