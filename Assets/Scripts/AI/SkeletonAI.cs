using System;
using System.Collections;
using System.Linq;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Tasks.Actions;
using CleverCrow.Fluid.BTs.Trees;
using Combat;
using Events;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using UnityEngine.Events;

namespace AI
{
    public class SkeletonAI : MonoBehaviour
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

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _weaponController = GetComponentInChildren<WeaponController>();
            _weaponController.SetDamage(1);
            _targetable = GetComponent<Targetable>();
            _targetable.InitHealth(4, 4);
            _animator = GetComponent<Animator>();
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
                            EventManager.TriggerEvent<SkeletonDeathEvent, Vector3>(transform.position);
                            return TaskStatus.Success;
                        })
                        .RepeatForever()
                            .Do(() => TaskStatus.Continue)
                        .End()
                    .End()
                    // Idle
                    .Sequence()
                        .Condition(() => _playerHasDied)
                        .Do(() =>
                        {
                            AnimatorTrigger(OnIdle);
                            return TaskStatus.Success;
                        })
                    .End()
                    // Knock back - UNUSED
                    /*.Sequence()
                        .Condition(() =>
                        {
                            return Physics.OverlapSphere(transform.position, .3f)
                                .Any(c => c.CompareTag("Player"));
                        })
                        .Sequence()
                            .Do(() =>
                            {
                                EventManager.TriggerEvent<AIAudioHandler.OrbAttackEvent>();

                                // move agent in opposite direction of player
                                var playerToSelf = transform.position - _player.transform.position;
                                GetComponent<Rigidbody>().isKinematic = false;
                                GetComponent<Rigidbody>().AddForce(playerToSelf.normalized * 3f, ForceMode.Impulse);

                                return TaskStatus.Success;
                            })
                            .WaitTime(1.1f)
                            .Do(() =>
                            {
                                GetComponent<Rigidbody>().isKinematic = true;
                                return TaskStatus.Success;
                            })
                        .End()
                    .End()*/
                    // Do nothing if path pending
                    .Sequence()
                        .Condition(() => _agent.pathPending)
                        .Do(() => TaskStatus.Success)
                    .End()
                    // Block
                    .Sequence()
                        .Condition(() => Vector3.Distance(_agent.transform.position, _player.transform.position) < ChaseDistance && !PlayerCloseAndInFrontForAttack() && _animator.GetCurrentAnimatorStateInfo(0).tagHash != Animator.StringToHash("ShortAttack") && _playerController.isAttacking == true)
                        .Do(() =>
                        {
                            isBlocking = true;
                            _agent.isStopped = true;
                            _agent.ResetPath();
                            _animator.applyRootMotion = true;
                            AnimatorTrigger(OnBlock);
                            return TaskStatus.Success;
                        })
                        .WaitTime(1.1f)
                    .End()
                    // Chase player
                    .Sequence()
                        .Condition(() => (Vector3.Distance(_agent.transform.position, _player.transform.position) < ChaseDistance || waypoints.Length == 0) && !PlayerCloseAndInFrontForAttack())
                        .Do(() =>
                        {
                            isBlocking = false;
                            _animator.applyRootMotion = false;
                            return TaskStatus.Success;
                        })
                        .SkeletonChasePlayer()
                    .End()
                    // Short Range Attack
                    .Sequence()
                        .Condition(() => PlayerCloseAndInFrontForAttack() && _animator.GetCurrentAnimatorStateInfo(0).tagHash != Animator.StringToHash("Block"))
                        .Do(() =>
                        {
                            _agent.isStopped = true;
                            _agent.ResetPath();
                            _animator.applyRootMotion = true;
                            AnimatorTrigger(OnShortRangeAttack);
                            return TaskStatus.Success;
                        })
                    .End()
                    // If waypoints doesn't exist (when spawned in the boss room), chase the player
                    .Sequence()
                        .Condition(() => waypoints.Length == 0)
                        .Do(() => {
                            isBlocking = false;
                            _animator.applyRootMotion = false;
                            return TaskStatus.Success;
                        })
                        .SkeletonChasePlayer()
                    .End() 
                    // Patrol
                    .SkeletonPatrol(waypoints)
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
    }

    public class SkeletonChasePlayerAction : ActionBase
    {
        private static readonly int HoveringSpeed = Animator.StringToHash("HoveringSpeed");

        private NavMeshAgent _agent;
        private GameObject _player;
        private Animator _animator;

        protected override void OnInit()
        {
            _agent = Owner.GetComponent<NavMeshAgent>();
            _player = GameObject.FindGameObjectWithTag("Player");
            _animator = Owner.GetComponent<Animator>();
        }

        protected override void OnStart()
        {
        }

        protected override TaskStatus OnUpdate()
        {
            _agent.SetDestination(_player.transform.position);
            return TaskStatus.Success;
        }
    }

    public class SkeletonPatrolAction : ActionBase
    {
        private static readonly int HoveringSpeed = Animator.StringToHash("HoveringSpeed");

        private NavMeshAgent _agent;
        private Animator _animator;
        private Transform _currentWaypointTransform;

        public GameObject[] waypoints;

        protected override void OnInit()
        {
            _agent = Owner.GetComponent<NavMeshAgent>();
            _animator = Owner.GetComponent<Animator>();

            if (waypoints.Length > 0)
            {
                _currentWaypointTransform = waypoints[Random.Range(0, waypoints.Length)].transform;
            }
        }

        protected override void OnStart()
        {

            if (_currentWaypointTransform != null)
            {
                _agent.SetDestination(_currentWaypointTransform.position);
            }
            else
            {
                _agent.ResetPath();
            }
        }

        protected override TaskStatus OnUpdate()
        {
            if (_agent.remainingDistance > 0.5f || waypoints.Length == 0)
            {
                return TaskStatus.Success;
            }

            _currentWaypointTransform = waypoints[Random.Range(0, waypoints.Length)].transform;
            _agent.SetDestination(_currentWaypointTransform.position);
            return TaskStatus.Success;
        }
    }

    public static class SkeletonBehaviorTreeBuilderExtensions
    {
        public static BehaviorTreeBuilder SkeletonChasePlayer(this BehaviorTreeBuilder builder)
        {
            return builder.AddNode(new SkeletonChasePlayerAction {});
        }

        public static BehaviorTreeBuilder SkeletonPatrol(this BehaviorTreeBuilder builder, GameObject[] waypoints)
        {
            return builder.AddNode(new SkeletonPatrolAction { waypoints = waypoints});
        }
    }
}
