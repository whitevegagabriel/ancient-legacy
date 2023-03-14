using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTriggers : MonoBehaviour
{
    private bool isCylinderMoving = false;

    public void onSwing()
    {
        EventManager.TriggerEvent<SwooshEvent,Vector3>(transform.position);
    }

    public void onPlatformStart()
    {
        EventManager.TriggerEvent<PlatformStartEvent, Vector3>(transform.position);
    }

    public void onPlatformStop()
    {
        EventManager.TriggerEvent<PlatformStopEvent, Vector3>(transform.position);
    }

    public void onCylinderMove()
    {
        if (isCylinderMoving == false)
        {
            EventManager.TriggerEvent<CylinderMoveEvent, Vector3>(transform.position);
            isCylinderMoving = true;
        }
    }
}
