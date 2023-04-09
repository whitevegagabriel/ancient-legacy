using UnityEngine;

namespace Collectables
{
    public class KeyController : MonoBehaviour, ICollectable
    {
        public new string name;
        public string Name => name;
    }
}