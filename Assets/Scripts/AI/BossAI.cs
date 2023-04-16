using System.Linq;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using Combat;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace AI
{
    public class BossAI : MonoBehaviour
    {
        [SerializeField] private BehaviorTree tree;

        private static readonly int OnIdle = Animator.StringToHash("OnIdle");
        private static readonly int OnChase = Animator.StringToHash("OnChase");
        private static readonly int OnShortRangeAttack = Animator.StringToHash("OnShortRangeAttack");
        private static readonly int OnLongRangeAttack = Animator.StringToHash("OnLongRangeAttack");
        private static readonly int OnDie = Animator.StringToHash("OnDie");
        private const float AttackDistance = 1.5f;
        private const float AttackAngle = 30;
        private const float LongRangeAttackTimer = 10;
        private const float WarmupTimer = 2;
        private GameObject _player;
        private bool _playerHasDied;
        private NavMeshAgent _agent;
        private Animator _animator;
        private WeaponController _weaponController;
        private float _lastLongRangeAttackTime;
        private float _longRangeAttackAnimationLength;
        private float _startTime;
        private Targetable _targetable;
        private UnityAction _onAttackPartway;
        private UnityAction _onAttackEnd;
        private UnityAction _onDamageGiven;
        private bool _animatingLongRangeAttack;
        
        public GameObject radialDamagePrefab;
        public GameObject relicPrefab;
        private static readonly int Speed = Animator.StringToHash("speed");
        public GameObject skeletonPrefab;
        public float skeletonSpawnCooldown;
        public float nextSkeletonSpawn;

        private void Awake()
        {
            _targetable = GetComponent<Targetable>();
            _targetable.InitHealth(20, 20);
            EventManager.StartListening<PlayerDeathEvent, Vector3>(_ => _playerHasDied = true);
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _weaponController = GetComponentInChildren<WeaponController>();
            _weaponController.SetDamage(1);
            _lastLongRangeAttackTime = Time.time;
            // TODO: This is a hack, use an event-based system instead to know when the animation is done
            _longRangeAttackAnimationLength = GetClipLength("Mutant Jump") - 1;
            _startTime = Time.time;
            skeletonSpawnCooldown = 8f;
            nextSkeletonSpawn = Time.time;

            tree = new BehaviorTreeBuilder(gameObject)
                .Selector()
                    // Die
                    .Sequence()
                        .Condition(() => _targetable.GetHealth() <= 0)
                        .Do(() =>
                        {
                            _agent.isStopped = true;
                            AnimatorTrigger(OnDie);
                            KillAllSkeletons();
                            GetComponent<CapsuleCollider>().enabled = false;
                            GetComponent<MeshCollider>().enabled = true;
                            var relic = Instantiate(relicPrefab, transform.position, Quaternion.identity);
                            relic.transform.localPosition = new Vector3(0, 0.5f, 0);
                            return TaskStatus.Success;
                        })
                        // After dying, the boss cannot enter any other state, as it will be looping here forever
                        .RepeatForever()
                            .Do(() => TaskStatus.Continue)
                        .End()
                    .End()
                    // Flee from the player
                    .Sequence()
                        .Condition(() => IsAlmostDead())
                        .Do(() =>
                        {
                            if (Time.time >= nextSkeletonSpawn) {
                                SpawnSkeleton();
                                nextSkeletonSpawn = Time.time + skeletonSpawnCooldown;
                            }
                            AnimatorTrigger(OnChase);
                            _animator.applyRootMotion = false;
                            _agent.speed = 6;
                            return TaskStatus.Success;
                        })
                        // Wait until previous state is done animating
                        .RepeatUntilSuccess()
                            .Condition(() => _animator.GetCurrentAnimatorStateInfo(0).tagHash == Animator.StringToHash("Chase"))
                        .End()
                        .Do(() => {
                            if ((_player.transform.localPosition - transform.localPosition).magnitude > 5) {
                                transform.LookAt(_player.transform);
                                return TaskStatus.Success;
                            }
                            _agent.SetDestination(PositionToMoveToward());
                            return TaskStatus.Success;
                        })

                    .End()
                    // Idle
                    .Sequence()
                        .Condition(() => _playerHasDied || Time.time - _startTime < WarmupTimer)
                        .Do(() =>
                        {
                            AnimatorTrigger(OnIdle);
                            return TaskStatus.Success;
                        })
                        .RepeatUntilSuccess()
                            // Allows the boss to exit idle state at start of game, but not after defeating player
                            .Condition(() => !_playerHasDied && Time.time - _startTime >= WarmupTimer)
                        .End()
                    .End()
                    // Long range attack
                    .Sequence()
                        .Condition(() => Time.time - _lastLongRangeAttackTime > LongRangeAttackTimer)
                        .Do(() =>
                        {
                            _agent.isStopped = true; 
                            _agent.ResetPath();
                            _animator.applyRootMotion = true;
                            AnimatorTrigger(OnLongRangeAttack);
                            return TaskStatus.Success;
                        })
                        .WaitTime(_longRangeAttackAnimationLength/2)
                        .Do(() =>
                        {
                            EventManager.TriggerEvent<AIAudioHandler.RadialAttackEvent>();
                            return TaskStatus.Success;
                        })
                        .WaitTime(_longRangeAttackAnimationLength/2)
                        .Do(() =>
                        {
                            _lastLongRangeAttackTime = Time.time;
                            Instantiate(radialDamagePrefab, transform.position, Quaternion.identity);
                            return TaskStatus.Success;
                        })
                    .End()
                    // Chase
                    .Sequence()
                        .Condition(() => !_agent.pathPending && !PlayerCloseAndInFrontForAttack())
                        .Do(() =>
                        {
                            AnimatorTrigger(OnChase);
                            _animator.applyRootMotion = false;
                            return TaskStatus.Success;
                        })
                        // Wait until previous state is done animating
                        .RepeatUntilSuccess()
                            .Condition(() => _animator.GetCurrentAnimatorStateInfo(0).tagHash == Animator.StringToHash("Chase"))
                        .End()
                        .Do(() => {
                            _agent.SetDestination(_player.transform.position);
                            return TaskStatus.Success;
                        })
                    .End()
                    // Short range attack
                    .Sequence()
                        .Condition(PlayerCloseAndInFrontForAttack)
                        .Do(() =>
                        {
                            _agent.isStopped = true;
                            _agent.ResetPath();
                            _animator.applyRootMotion = true;
                            AnimatorTrigger(OnShortRangeAttack);
                            return TaskStatus.Success;
                        })
                    .End()
                .End()
                .Build();
        }

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
            _animator.SetFloat(Speed, _agent.velocity.magnitude/2f);
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
            _animator.ResetTrigger(OnLongRangeAttack);
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

       private Vector3 PositionToMoveToward() {
            Vector3 moveToward = new Vector3(0, 0, 0);
            float greatestDistance = 0;
            Vector3[] directions = new Vector3[] {
                new Vector3(1, 0, 0),
                new Vector3(-1, 0, 0),
                new Vector3(0, 0, 1),
                new Vector3(0, 0, -1),
                new Vector3(1, 0, 1),
                new Vector3(1, 0, -1),
                new Vector3(-1, 0, 1),
                new Vector3(-1, 0, -1)
            };
            for (int i = 0; i < 8; i++) {
                if (IsOutOfBounds(this.transform.localPosition + directions[i])) {
                    continue;
                }
                if ((_player.transform.localPosition - (this.transform.localPosition + directions[i])).magnitude > greatestDistance) {
                    moveToward = this.transform.localPosition + directions[i];
                    greatestDistance = (_player.transform.localPosition - (this.transform.localPosition + directions[i])).magnitude;
                }
            }
            return moveToward;
       }

       private bool IsOutOfBounds(Vector3 position) {
            NavMeshHit hit;
            return NavMesh.Raycast(transform.position, position, out hit, NavMesh.AllAreas);
       }

       private bool IsAlmostDead() {
            return _targetable.GetHealth() <= (_targetable.GetMaxHealth() / 3);
       }

       private void SpawnSkeleton() {
            Vector3 spawnPosition = transform.localPosition;
            spawnPosition.x += Random.Range(0, 1);
            spawnPosition.z += Random.Range(0, 1);
            Instantiate(skeletonPrefab, spawnPosition, Quaternion.identity);
       }

       private void KillAllSkeletons() {
            GameObject[] skeletons = GameObject.FindGameObjectsWithTag("Skeleton");
            foreach (var skeleton in skeletons) {
                Destroy(skeleton);
            }
       }
    }
}