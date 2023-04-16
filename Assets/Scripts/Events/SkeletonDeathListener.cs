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
            EventManager.StartListening<SkeletonDeathEvent>(OnSkeletonDeath);
        }
        
        private void OnSkeletonDeath()
        {
            _deadSkeletons++;
            
            if (_deadSkeletons < 2) return;
            
            EventManager.StopListening<SkeletonDeathEvent>(OnSkeletonDeath);
            var key = Instantiate(keyPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.Euler(104, 61, 33));
            key.GetComponent<KeyController>().name = "Key1";
        }
    }
}