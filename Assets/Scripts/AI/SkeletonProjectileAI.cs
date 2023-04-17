using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using Combat;
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
        
        private NavMeshAgent _agent;
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
        private bool trigger = true;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
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
                            return TaskStatus.Success;
                        })
                        .RepeatForever()
                            .Do(() => TaskStatus.Continue)
                        .End()
                    .End()
                    // Throw Projectile
                    .Sequence()
                        .Condition(() => Time.time - _projectileFireTime > projectileTimer)
                        .Do(() =>
                        {
                            AnimatorTrigger(OnShortRangeAttack);
                            _projectileFireTime = Time.time;
                            trigger = true;
                            return TaskStatus.Success;
                        })
                    .End()
                    // Do nothing if path pending
                    .Sequence()
                        .Condition(() => _agent.pathPending)
                        .Do(() => TaskStatus.Success)
                    .End()
                    .Sequence()
                        .Do(() =>
                        {
                            this.transform.LookAt(new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z));
                            if (_animator.GetCurrentAnimatorStateInfo(0).tagHash == Animator.StringToHash("Projectile") && trigger == true)
                            {
                                FireProjectile();
                                trigger = false;
                            }
                            return TaskStatus.Success;
                        })
                    .End()
                .End()
                .Build();
        }

        // Start is called before the first frame update
        void Start()
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

        void Update()
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

        void FireProjectile()
        {
            Rigidbody newProj = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), this.transform.rotation);
            newProj.velocity = newProj.transform.forward * speed;
        }
    }
}
