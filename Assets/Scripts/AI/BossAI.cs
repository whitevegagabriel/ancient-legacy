using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace AI
{
    public class BossAI : MonoBehaviour
    {
        static readonly float ChaseDistance = 10;
        static readonly float AttackDistance = 2;
        static readonly float AttackTimer = 1;
        static readonly float WarmupTimer = 2;
        
        GameObject _player;
        PlayerHealth _playerHealth;
        BossState _currentState;
        NavMeshAgent _agent;
        Animator _animator;
        BossHealthUI _healthDisplay;
        float _lastAttackTime;
        float _health;
        float _startTime;

        public enum BossState
        {
            Idle,
            Chase,
            Attack,
            Die,
            None,
        }
    
        public float GetHealth() {
            return _health;
        }

        void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _playerHealth = _player.GetComponent<PlayerHealth>();
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            SetState(BossState.Idle);
            _lastAttackTime = Time.time;
            _health = 10;
            _startTime = Time.time;
            _healthDisplay = GameObject.FindGameObjectWithTag("Boss Health Display").GetComponent<BossHealthUI>();
            _healthDisplay.SetHearts();
        }
    
        void Update()
        {
            if (Time.time - _startTime < WarmupTimer)
            {
                return;
            }
            
            if (_agent.pathPending)
            {
                return;
            }
            
            switch (_currentState)
            {
                case BossState.Idle:
                    HandleIdle();
                    break;
                case BossState.Chase:
                    HandleChase();
                    break;
                case BossState.Attack:
                    HandleAttack();
                    break;
                case BossState.Die:
                    HandleDie();
                    break;
            }
        }
    
        void HandleIdle()
        {
            if (_health <= 0)
            {
                SetState(BossState.Die);
                return;
            }

            if (_agent.hasPath)
            {
                _agent.ResetPath();
            }
            
            if (Vector3.Distance(transform.position, _player.transform.position) < ChaseDistance)
            {
                SetState(BossState.Chase);
            }
        }
        
        void HandleChase()
        {
            if (_health <= 0)
            {
                SetState(BossState.Die);
                return;
            }
            
            if (Vector3.Distance(transform.position, _player.transform.position) > ChaseDistance)
            {
                SetState(BossState.Idle);
                return;
            }
            
            _agent.SetDestination(_player.transform.position);
            if (Vector3.Distance(transform.position, _player.transform.position) < AttackDistance)
            {
                SetState(BossState.Attack);
            }
        }
        
        void HandleAttack()
        {
            if (_health <= 0)
            {
                SetState(BossState.Die);
                return;
            }
            
            if (_agent.hasPath)
            {
                _agent.ResetPath();
            }

            if (Vector3.Distance(transform.position, _player.transform.position) > AttackDistance)
            {
                SetState(BossState.Chase);
                return;
            }
            
            if (Time.time - _lastAttackTime > AttackTimer)
            {

                _lastAttackTime = Time.time;
                Debug.Log("Attack");
                
                _playerHealth.DecreaseHealth(1);
                Debug.Log("Player took damage");
            }
        }
        
        void HandleDie()
        {
            if (_agent.hasPath)
            {
                _agent.ResetPath();
            }
            
            Debug.Log("Boss died");
            SetState(BossState.None);
        }

        public void SetState(BossState state)
        {
            Debug.Log("Set state to " + state);
            _currentState = state;
            ResetTriggers();
            switch (state)
            {
                case BossState.Idle:
                    _animator.SetTrigger("OnIdle");
                    break;
                case BossState.Chase:
                    _animator.SetTrigger("OnChase");
                    break;
                case BossState.Attack:
                    _animator.SetTrigger("OnAttack");
                    break;
                case BossState.Die:
                    _animator.SetTrigger("OnDie");
                    break;
            }
        }
        
        void ResetTriggers()
        {
            _animator.ResetTrigger("OnIdle");
            _animator.ResetTrigger("OnChase");
            _animator.ResetTrigger("OnAttack");
            _animator.ResetTrigger("OnDie");
        }
    }
}