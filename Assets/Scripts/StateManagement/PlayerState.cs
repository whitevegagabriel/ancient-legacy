using SceneManagement;
using UnityEngine;

namespace StateManagement
{
    public static class PlayerState
    {
        public static int JumpCount { get; set; } = 3;
        public static int RunCount { get; set; }

        public static int SavedHealth { get; set; }

        public static bool Initialized;
    }
}
