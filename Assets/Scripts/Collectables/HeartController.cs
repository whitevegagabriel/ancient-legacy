using UnityEngine;

namespace Collectables
{
    public class HeartController : MonoBehaviour, ICollectable
    {
        public const string CollectableName = "ExtraHeart";
        public string Name => CollectableName;
    }
}