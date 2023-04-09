using System;
using Collectables;
using UnityEngine;

namespace Events
{
    public class SkeletonDeathListener : MonoBehaviour
    {
        public GameObject keyPrefab;
        private int _deadSkeletons;
        
        private void Start()
        {
            EventManager.StartListening<SkeletonDeathEvent, Vector3>(OnSkeletonDeath);
        }
        
        private void OnSkeletonDeath(Vector3 position)
        {
            _deadSkeletons++;
            
            if (_deadSkeletons < 2) return;
            
            EventManager.StopListening<SkeletonDeathEvent, Vector3>(OnSkeletonDeath);
            var key = Instantiate(keyPrefab, position + new Vector3(0, 1, 0), Quaternion.Euler(104, 61, 33));
            key.GetComponent<KeyController>().name = "Key1";
        }
    }
}