using UnityEngine;

namespace AI
{
    public class Projectile : MonoBehaviour
    {
        private WeaponController _weaponController;
        private SkeletonProjectileAI _skeletonUser;
        private int frames;
        private bool frameCount;
        private Renderer rend;

        private void Start()
        {
            _weaponController = GetComponent<WeaponController>();
            _weaponController.SetDamage(1);
            rend = GetComponent<Renderer>();
            frames = 0;
            frameCount = false;
            _weaponController.StartAttack();
            EventManager.TriggerEvent<FireballThrowEvent, GameObject>(gameObject);
        }

        private void Update()
        {
            if (!frameCount) return;
            frames += 1;
            if (frames >= 20)
            {
                gameObject.SetActive(false);
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            switch (other.gameObject.tag)
            {
                case "ProjectileUser":
                    return;
                case "Player":
                    rend.enabled = false;
                    frameCount = true;
                    break;
                default:
                    EventManager.TriggerEvent<FireballWallEvent, Vector3>(transform.position);
                    gameObject.SetActive(false);
                    break;
            }
        }
    }
}
