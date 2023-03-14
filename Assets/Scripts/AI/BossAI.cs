using System;
using System.Collections;
using Combat;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace AI
{
    public class BossAI : MonoBehaviour
    {
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
        private BossState _currentState;
        private NavMeshAgent _agent;
        private Animator _animator;
        private WeaponController _weaponController;
        private float _lastLongRangeAttackTime;
        private float _longRangeAttackAnimationLength;
        private float _shortRangeAttackAnimationLength;
        private float _startTime;
        private Targetable _targetable;
        private UnityAction _onAttackStart;
        private UnityAction _onAttackEnd;
        
        public GameObject radialDamagePrefab;
        public GameObject relicPrefab;

        private enum BossState
        {
            Idle,
            Chase,
            ShortRangeAttack,
            LongRangeAttack,
            Die,
            None
        }

        private void Awake()
        {
            _targetable = GetComponent<Targetable>();
            _targetable.InitHealth(20, 20);
        }

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _weaponController = GetComponentInChildren<WeaponController>();
            _weaponController.SetDamage(1);
            SetState(BossState.Idle);
            _lastLongRangeAttackTime = Time.time;
            // TODO: This is a hack, use an event-based system instead to know when the animation is done
            _longRangeAttackAnimationLength = GetClipLength("Mutant Jump") - 1;
            _startTime = Time.time;
            
            _onAttackStart = () =>
            {
                _weaponController.StartAttack();
            };
            _onAttackEnd = () =>
            {
                _weaponController.StopAttack();
            };
            ShortRangeAttackDetection.AddAttackCallback(new AttackCallback(_onAttackStart, _onAttackEnd));
        }

        private void Update()
        {
            if (_agent.pathPending) return;

            switch (_currentState)
            {
                case BossState.Idle:
                    HandleIdle();
                    break;
                case BossState.Chase:
                    HandleChase();
                    break;
                case BossState.ShortRangeAttack:
                    HandleShortRangeAttack();
                    break;
                case BossState.LongRangeAttack:
                    HandleLongRangeAttack();
                    break;
                case BossState.Die:
                    HandleDie();
                    break;
                case BossState.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleIdle()
        {
            // Boss is only idle at the start of the scene
            if (Time.time - _startTime > WarmupTimer) SetState(BossState.Chase);
        }

        private void HandleChase()
        {
            if (_weaponController.isAttacking) return;
            
            if (_targetable.GetHealth() <= 0)
            {
                SetState(BossState.Die);
                return;
            }

            if (Time.time - _lastLongRangeAttackTime > LongRangeAttackTimer)
            {
                SetState(BossState.LongRangeAttack);
                return;
            }

            _agent.SetDestination(_player.transform.position);

            if (Vector3.Distance(transform.position, _player.transform.position) <= AttackDistance)
                SetState(BossState.ShortRangeAttack);
        }

        private void HandleShortRangeAttack()
        {
            if (_targetable.GetHealth() <= 0)
            {
                SetState(BossState.Die);
                return;
            }
            
            if (Time.time - _lastLongRangeAttackTime > LongRangeAttackTimer)
            {
                SetState(BossState.LongRangeAttack);
                return;
            }

            if (!PlayerCloseAndInFrontForAttack())
            {
                SetState(BossState.Chase);
            }
        }

        private void HandleLongRangeAttack()
        {
            if (_targetable.GetHealth() <= 0)
            {
                SetState(BossState.Die);
                return;
            }
            
            // wait for LongRangeAttack animation to finish, then attack
            if (Time.time - _lastLongRangeAttackTime < LongRangeAttackTimer + _longRangeAttackAnimationLength) return;
            
            _lastLongRangeAttackTime = Time.time;
            Instantiate(radialDamagePrefab, transform.position, Quaternion.identity);
            Debug.Log("Long range attack");

            SetState(Vector3.Distance(transform.position, _player.transform.position) <= AttackDistance
            ? BossState.ShortRangeAttack
            : BossState.Chase);
        }

        private void HandleDie()
        {
            Debug.Log("Boss died");
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            GetComponent<Collider>().enabled = false;
            GameObject go = Instantiate(relicPrefab, transform.position, Quaternion.identity);
            go.transform.localPosition = new Vector3(0, 0.5f, 0);
            SetState(BossState.None);
        }

        private void SetState(BossState state)
        {
            Debug.Log("Set state to " + state);
            if (_agent.hasPath)
            {
                _agent.isStopped = true;
                _agent.ResetPath();
            }
            _currentState = state;
            ResetTriggers();
            switch (state)
            {
                case BossState.Idle:
                    _animator.SetTrigger(OnIdle);
                    break;
                case BossState.Chase:
                    _animator.SetTrigger(OnChase);
                    break;
                case BossState.ShortRangeAttack:
                    _animator.SetTrigger(OnShortRangeAttack);
                    break;
                case BossState.LongRangeAttack:
                    _animator.SetTrigger(OnLongRangeAttack);
                    break;
                case BossState.Die:
                    _animator.SetTrigger(OnDie);
                    break;
                case BossState.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
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
            foreach (AnimationClip clip in _animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == clipName)
                {
                    return clip.length;
                }
            }

            return 0;
        }

        private bool PlayerCloseAndInFrontForAttack()
        {
            var angle = Vector3.Angle(_player.transform.position - _agent.transform.position, _agent.transform.forward);
            var distance = Vector3.Distance(_player.transform.position, _agent.transform.position);
            return distance <= AttackDistance && angle <= AttackAngle;
        }
    }
}