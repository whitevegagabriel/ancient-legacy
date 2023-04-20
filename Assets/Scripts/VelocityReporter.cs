using UnityEngine;

public class VelocityReporter : MonoBehaviour
{
    private Vector3 prevPos;

    public Vector3 RawVelocity
    {
        get;
        private set;
    }
    
    public Vector3 Velocity
    {
        get;
        private set;
    }

    public float smoothingTimeFactor = 0.5f;

    private Vector3 smoothingParamVel;
    // Start is called before the first frame update
    void Start()
    {
        prevPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Mathf.Approximately(Time.deltaTime, 0f))
        {
            RawVelocity = (transform.position - prevPos) / Time.deltaTime;
            Velocity = Vector3.SmoothDamp(Velocity, RawVelocity, ref smoothingParamVel, smoothingTimeFactor);
        }
        else
        {
            RawVelocity = Vector3.zero;
            Velocity = Vector3.zero;
        }
        prevPos = transform.position;
    }
}