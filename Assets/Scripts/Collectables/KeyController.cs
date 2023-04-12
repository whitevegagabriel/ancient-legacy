using UnityEngine;

namespace Collectables
{
    public class KeyController : MonoBehaviour, ICollectable, ICheckpoint
    {
        public new string name;
        public string Name => name;
        
        public ICheckpoint.CheckpointData GetCheckPointData()
        {
            return new ICheckpoint.CheckpointData(transform.position);
        }
    }
}