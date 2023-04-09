using UnityEngine;

namespace Walkways
{
    public class ControlledMovingPlatform : MonoBehaviour
    {
        public GameObject platform;
        public static bool isMoving = false;
        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private float _offset = 0.0f;
        
        private void Start()
        {
            _startPosition = platform.transform.position;
            _endPosition = _startPosition + new Vector3(5.79f, 0f, -3.21f);
        }
        
        private void FixedUpdate()
        {
            if (!isMoving)
            {
                platform.GetComponent<AudioSource>().mute = true;
                return;
            }
            _offset += Time.deltaTime / 4;
            platform.GetComponent<AudioSource>().mute = false;
            platform.transform.position = Vector3.Lerp(_startPosition, _endPosition, Mathf.PingPong(_offset, 1));
        }
    }
}