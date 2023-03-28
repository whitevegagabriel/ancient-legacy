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

namespace AI
{
    public class OrbAI : MonoBehaviour
    {
        public GameObject[] waypoints;

        [SerializeField] private BehaviorTree tree;

        private const float ChaseDistance = 7f;
        private NavMeshAgent _agent;
        private GameObject _player;
        private WeaponController _weaponController;
        private Targetable _targetable;
        private Animator _animator;
        private static readonly int HoveringOffset = Animator.StringToHash("HoveringOffset");
        private float _hoveringSpeed;
        private int _currentWaypoint;
        
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _weaponController = GetComponentInChildren<WeaponController>();
            _weaponController.SetDamage(1);
            _targetable = GetComponent<Targetable>();
            _targetable.InitHealth(2, 2);
            _animator = GetComponent<Animator>();
            // a random offset to make the orbs look more independent
            _animator.SetFloat(HoveringOffset, Random.Range(0f, 1f));
            _hoveringSpeed = Random.Range(0.8f, 1.2f);
            
            tree = new BehaviorTreeBuilder(gameObject)
                .Selector()
                    // Die
                    .Sequence()
                        .Condition(() => _targetable.GetHealth() <= 0)
                        .Do(() =>
                        {
                            EventManager.TriggerEvent<AIAudioHandler.OrbDeathEvent>();
                            Destroy(gameObject);
                            return TaskStatus.Success;
                        })
                    .End()
                    // Knock back
                    .Sequence()
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
                    .End()
                    // Do nothing if path pending
                    .Sequence()
                        .Condition(() => _agent.pathPending)
                        .Do(() => TaskStatus.Success)
                    .End()
                    // Chase player
                    .Sequence()
                        .Condition(() => Vector3.Distance(_agent.transform.position, _player.transform.position) < ChaseDistance)
                        .ChasePlayer(_hoveringSpeed)
                    .End()
                    // Patrol
                    .Patrol(waypoints, _hoveringSpeed)
                .End()
                .Build();
        }

        // Start is called before the first frame update
        void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        void Update()
        {
            tree.Tick();
        }
    }
    
    public class ChasePlayerAction : ActionBase
    {
        private static readonly int HoveringSpeed = Animator.StringToHash("HoveringSpeed");
        
        private NavMeshAgent _agent;
        private GameObject _player;
        private Animator _animator;
        private WeaponController _weaponController;
        
        public float hoveringSpeed;
        
        protected override void OnInit () 
        {
            _agent = Owner.GetComponent<NavMeshAgent>();
            _player = GameObject.FindGameObjectWithTag("Player");
            _animator = Owner.GetComponent<Animator>();
            _weaponController = Owner.GetComponentInChildren<WeaponController>();
        }

        protected override void OnStart () {
            _animator.SetFloat(HoveringSpeed, hoveringSpeed * 1.4f);
            _weaponController.StartAttack();
        }

        protected override TaskStatus OnUpdate()
        {
            _agent.SetDestination(_player.transform.position);
            return TaskStatus.Success;
        }
    }

    public class PatrolAction : ActionBase
    {
        private static readonly int HoveringSpeed = Animator.StringToHash("HoveringSpeed");
        
        private NavMeshAgent _agent;
        private Animator _animator;
        private Transform _currentWaypointTransform;
        
        public GameObject[] waypoints;
        public float hoveringSpeed;
        
        protected override void OnInit () 
        {
            _agent = Owner.GetComponent<NavMeshAgent>();
            _animator = Owner.GetComponent<Animator>();
            
            if (waypoints.Length > 0)
            {
                _currentWaypointTransform = waypoints[Random.Range(0, waypoints.Length)].transform;
            }
        }
        
        protected override void OnStart () 
        {
            _animator.SetFloat(HoveringSpeed, hoveringSpeed);
            
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
    
    public static class BehaviorTreeBuilderExtensions
    {
        public static BehaviorTreeBuilder ChasePlayer(this BehaviorTreeBuilder builder, float hoveringSpeed)
        {
            return builder.AddNode(new ChasePlayerAction { hoveringSpeed = hoveringSpeed });
        }

        public static BehaviorTreeBuilder Patrol(this BehaviorTreeBuilder builder, GameObject[] waypoints,
            float hoveringSpeed)
        {
            return builder.AddNode(new PatrolAction { waypoints = waypoints, hoveringSpeed = hoveringSpeed });
        }
    }
}
