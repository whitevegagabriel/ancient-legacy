using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class BossAI : MonoBehaviour
    {
        static readonly float ChaseDistance = 10;
        static readonly float AttackDistance = 2;
        static readonly float AttackTimer = 1;
        
        public GameObject player;
        BossState _currentState;
        NavMeshAgent _agent;
        float _lastAttackTime;
        float _health;
    
        public enum BossState
        {
            Idle,
            Chase,
            Attack,
            Die,
            None,
        }
    
        // Start is called before the first frame update
        void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _currentState = BossState.Idle;
            _lastAttackTime = Time.time;
            _health = 10;
        }
    
        // Update is called once per frame
        void Update()
        {
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
            
            if (Vector3.Distance(transform.position, player.transform.position) < ChaseDistance)
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
            
            if (Vector3.Distance(transform.position, player.transform.position) > ChaseDistance)
            {
                SetState(BossState.Idle);
                return;
            }
            
            _agent.SetDestination(player.transform.position);
            if (Vector3.Distance(transform.position, player.transform.position) < AttackDistance)
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

            if (Vector3.Distance(transform.position, player.transform.position) > AttackDistance)
            {
                SetState(BossState.Chase);
                return;
            }
            
            if (Time.time - _lastAttackTime > AttackTimer)
            {
                _lastAttackTime = Time.time;
                Debug.Log("Attack");
                
                TakeDamage(Random.value * 4);
                Debug.Log("Took random damage");
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
        
        public void TakeDamage(float damage)
        {
            _health -= damage;
        }
        
        public void SetState(BossState state)
        {
            Debug.Log("Set state to " + state);
            _currentState = state;
        }
    }
}