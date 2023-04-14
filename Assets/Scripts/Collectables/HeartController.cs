using System;
using System.Collections;
using UnityEngine;

namespace Collectables
{
    public class HeartController : MonoBehaviour, ICollectable, ICheckpoint
    {
        public const string CollectableName = "ExtraHeart";
        public string Name => CollectableName;
        private DisplayOneUpEvent _displayOneUpEvent = DisplayOneUpEvent.Instance;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _displayOneUpEvent.Invoke();
            }
        }

        public ICheckpoint.CheckpointData GetCheckPointData()
        {
            return new ICheckpoint.CheckpointData(transform.position);
        }
    }
}