using System;
using System.Collections;
using System.Linq;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using CleverCrow.Fluid.BTs.Trees;
using Combat;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using UnityEngine.Events;

namespace AI
{
    public class SkeletonProjectileAI : MonoBehaviour
    {
        public GameObject[] waypoints;

        private static readonly int OnDie = Animator.StringToHash("OnDie");
        private static readonly int OnShortRangeAttack = Animator.StringToHash("OnShortRangeAttack");
        private static readonly int OnChase = Animator.StringToHash("OnChase");
        private static readonly int OnIdle = Animator.StringToHash("OnIdle");
        private static readonly int OnBlock = Animator.StringToHash("OnBlock");

        [SerializeField] private BehaviorTree tree;

        private const float ChaseDistance = 7f;
        private const float AttackDistance = 1.5f;
        private const float AttackAngle = 30;
        private bool _playerHasDied;
        private NavMeshAgent _agent;
        private GameObject _player;
        private PlayerController _playerController;
        private WeaponController _weaponController;
        private Targetable _targetable;
        private Animator _animator;
        private int _currentWaypoint;
        private UnityAction _onAttackPartway;
        private UnityAction _onAttackEnd;
        private UnityAction _onDamageGiven;
        public bool isBlocking = false;
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
            EventManager.StartListening<PlayerDeathEvent, Vector3>(_ => _playerHasDied = true);

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
            _playerController = _player.GetComponent<PlayerController>();
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
            _animator.ResetTrigger(OnIdle);
            _animator.ResetTrigger(OnChase);
            _animator.ResetTrigger(OnShortRangeAttack);
            _animator.ResetTrigger(OnBlock);
            _animator.ResetTrigger(OnDie);
        }

        private float GetClipLength(string clipName)
        {
            return (from clip in _animator.runtimeAnimatorController.animationClips where clip.name == clipName select clip.length).FirstOrDefault();
        }

        private bool PlayerCloseAndInFrontForAttack()
        {
            var angle = Vector3.Angle(_player.transform.position - _agent.transform.position, _agent.transform.forward);
            var distance = Vector3.Distance(_player.transform.position, _agent.transform.position);
            return distance <= AttackDistance && angle <= AttackAngle;
        }

        void FireProjectile()
        {
            Rigidbody newProj = Instantiate(projectile, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), this.transform.rotation);
            newProj.velocity = newProj.transform.forward * speed;
        }
    }
}
