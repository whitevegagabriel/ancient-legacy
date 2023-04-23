using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using Combat;
using Events;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace AI
{
    public class SkeletonProjectileAI : MonoBehaviour
    {
        private static readonly int OnDie = Animator.StringToHash("OnDie");
        private static readonly int OnShortRangeAttack = Animator.StringToHash("OnShortRangeAttack");

        [SerializeField] private BehaviorTree tree;
        
        private GameObject _player;
        private WeaponController _weaponController;
        private Targetable _targetable;
        private Animator _animator;
        private int _currentWaypoint;
        private UnityAction _onAttackPartway;
        private UnityAction _onAttackEnd;
        private UnityAction _onDamageGiven;
        private float _projectileFireTime;
        private float projectileTimer = 5;
        public Rigidbody projectile;
        public float speed;

        private void Awake()
        {
            _weaponController = GetComponentInChildren<WeaponController>();
            _weaponController.SetDamage(1);
            _targetable = GetComponent<Targetable>();
            _targetable.InitHealth(2, 2);
            _animator = GetComponent<Animator>();
            _projectileFireTime = Time.time;

            tree = new BehaviorTreeBuilder(gameObject)
                .Selector()
                    // Die
                    .Sequence()
                        .Condition(() => _targetable.GetHealth() <= 0)
                        .Do(() =>
                        {
                            AnimatorTrigger(OnDie);
                            GetComponent<CapsuleCollider>().enabled = false;
                            GetComponent<MeshCollider>().enabled = true;
                            GetComponent<Rigidbody>().isKinematic = false;
                            EventManager.TriggerEvent<SkeletonDeathEvent>();
                            return TaskStatus.Success;
                        })
                        .RepeatForever()
                            .Do(() => TaskStatus.Continue)
                        .End()
                    .End()
                    .Sequence()
                        // Look at player
                        .Do(() =>
                        {
                            transform.LookAt(new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z));
                            return TaskStatus.Success;
                        })
                        // Throw Projectile
                        .Condition(() => Time.time - _projectileFireTime > projectileTimer)
                        .Condition(PlayerInLineOfSight)
                        .Do(() =>
                        {
                            AnimatorTrigger(OnShortRangeAttack);
                            _projectileFireTime = Time.time;
                            return TaskStatus.Success;
                        })
                        .RepeatUntilSuccess()
                            .Condition(() => _animator.GetCurrentAnimatorStateInfo(0).tagHash == Animator.StringToHash("Projectile"))
                        .End()
                        .Do(() =>
                        {
                            FireProjectileAtPlayer();
                            return TaskStatus.Success;
                        })
                    .End()
                .End()
                .Build();
        }

        // Start is called before the first frame update
        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _onAttackPartway = () =>
            {
                _weaponController.StartAttack();
                EventManager.TriggerEvent<AIAudioHandler.BossPunchEvent>();
            };
            _onAttackEnd = () =>
            {
                _weaponController.StopAttack();
            };
            ShortRangeAttackDetection.AddAttackCallback(new AttackCallback(_onAttackPartway, _onAttackEnd));
        }

        private void Update()
        {
            tree.Tick();
        }

        private void AnimatorTrigger(int id)
        {
            ResetTriggers();
            _animator.SetTrigger(id);
        }

        private void ResetTriggers()
        {
            _animator.ResetTrigger(OnShortRangeAttack);
            _animator.ResetTrigger(OnDie);
        }

        private bool PlayerInLineOfSight()
        {
            var aiToPlayerVector = _player.transform.position - transform.position;
            var ret = Physics.Raycast(transform.position + Vector3.up, aiToPlayerVector, out var hit,
                aiToPlayerVector.magnitude + 0.5f) && 
                      (hit.transform.gameObject.CompareTag("Player") || hit.transform.gameObject.CompareTag("PlayerShield"));
            Debug.DrawLine(transform.position + Vector3.up, hit.transform.position, Color.red, 2);
            return ret;
        }
        
        private void FireProjectileAtPlayer()
        {
            var aiToPlayerVector = _player.transform.position - transform.position;

            var newProj = Instantiate(projectile, transform.position + Vector3.up, Quaternion.identity);
            newProj.velocity = aiToPlayerVector.normalized * speed;
        }
    }
}
