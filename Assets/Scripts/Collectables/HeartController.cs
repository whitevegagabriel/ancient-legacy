using UnityEngine;

namespace Collectables
{
    public class HeartController : MonoBehaviour, ICollectable, ICheckpoint
    {
        public const string CollectableName = "ExtraHeart";
        public string Name => CollectableName;
        
        public ICheckpoint.CheckpointData GetCheckPointData()
        {
            return new ICheckpoint.CheckpointData(transform.position);
        }
    }
}