using System;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class BossAI : MonoBehaviour
    {
        private static readonly int OnIdle = Animator.StringToHash("OnIdle");
        private static readonly int OnChase = Animator.StringToHash("OnChase");
        private static readonly int OnShortRangeAttack = Animator.StringToHash("OnShortRangeAttack");
        private static readonly int OnLongRangeAttack = Animator.StringToHash("OnLongRangeAttack");
        private static readonly int OnDie = Animator.StringToHash("OnDie");
        private const float AttackDistance = 2;
        private const float ShortRangeAttackTimer = 1;
        private const float LongRangeAttackTimer = 5;
        private const float WarmupTimer = 2;

        private GameObject _player;
        private PlayerHealth _playerHealth;
        private BossHealthUI _healthDisplay;
        private BossState _currentState;
        private NavMeshAgent _agent;
        private Animator _animator;
        private float _lastShortRangeAttackTime;
        private float _lastLongRangeAttackTime;
        private float _longRangeAttackAnimationLength;
        private float _health;
        private float _startTime;
        
        public GameObject radialDamagePrefab;

        private enum BossState
        {
            Idle,
            Chase,
            ShortRangeAttack,
            LongRangeAttack,
            Die,
            None
        }

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _playerHealth = _player.GetComponent<PlayerHealth>();
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            SetState(BossState.Idle);
            _lastShortRangeAttackTime = Time.time;
            _lastLongRangeAttackTime = Time.time;
            // TODO: This is a hack, use an event-based system instead to know when the animation is done
            _longRangeAttackAnimationLength = GetClipLength("Mutant Jump") - 1;
            _health = 10;
            _startTime = Time.time;
            _healthDisplay = GameObject.FindGameObjectWithTag("Boss Health Display").GetComponent<BossHealthUI>();
            if (_healthDisplay == null) Debug.Log("not found");
            else {
                _healthDisplay.SetHearts((int) _health);
            }
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
            if (_health <= 0)
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
            if (_health <= 0)
            {
                SetState(BossState.Die);
                return;
            }
            
            if (Time.time - _lastLongRangeAttackTime > LongRangeAttackTimer)
            {
                SetState(BossState.LongRangeAttack);
                return;
            }

            if (Vector3.Distance(transform.position, _player.transform.position) > AttackDistance)
            {
                SetState(BossState.Chase);
                return;
            }

            if (!(Time.time - _lastShortRangeAttackTime > ShortRangeAttackTimer)) return;

            _agent.SetDestination(_player.transform.position);
            _lastShortRangeAttackTime = Time.time;
            Debug.Log("Attack");

            _playerHealth.DecreaseHealth(1);
        }

        private void HandleLongRangeAttack()
        {
            if (_health <= 0)
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
        
        public float GetHealth() {
            Debug.Log("Got health");
            return _health;
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
    }
}