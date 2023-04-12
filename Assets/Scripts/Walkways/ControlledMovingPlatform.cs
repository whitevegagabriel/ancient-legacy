using UnityEngine;

namespace Walkways
{
    public class ControlledMovingPlatform : MonoBehaviour
    {
        public GameObject platform;
        public static bool isMoving = false;
        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private float _offset;
        private readonly ResetEvent resetEvent = ResetEvent.Instance;
        private readonly SaveEvent saveEvent = SaveEvent.Instance;

        private void Start()
        {
            _startPosition = platform.transform.position;
            _endPosition = _startPosition + new Vector3(5.79f, 0f, -3.21f);
            saveEvent.AddListener(Save);
            resetEvent.AddListener(Reset);
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
        
        private void Save() {
            Debug.Log("Saving");
            _startPosition = platform.transform.position;
        }
        
        private void Reset() {
            Debug.Log("Resetting");
            platform.transform.position = _startPosition;
            _offset = 0f;
        }
    }
}