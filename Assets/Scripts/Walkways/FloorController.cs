using System.Collections;
using UnityEngine;

namespace Walkways
{
    public class FloorController : MonoBehaviour
    {
        private bool isFalling;
        private float downSpeed ;
        private Vector3 originalPosition;
        private readonly ResetEvent resetEvent = ResetEvent.Instance;

        private void Start() {
            originalPosition = transform.localPosition;
            resetEvent.AddListener(ResetTile);
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (!collision.gameObject.CompareTag("Player") || isFalling) return;
            
            isFalling = true;
        }

        private void FixedUpdate()
        {
            if (!isFalling) return;
        
            downSpeed += Time.deltaTime/12;
            transform.position -= new Vector3(0, downSpeed, 0);

        }

        private void ResetTile() {
            isFalling = false;
            downSpeed = 0f;
            transform.localPosition = originalPosition;
        }
    }
}