using System.Collections;
using Combat;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class OrbAI : MonoBehaviour
    {
        public GameObject[] waypoints;

        private float _chaseDistance = 7f;
        private State _state;
        private NavMeshAgent _agent;
        private GameObject _player;
        private int _currentWaypoint;
        private WeaponController _weaponController;
        private Targetable _targetable;

        private enum State
        {
            Patrol,
            Chase,
            Knockback,
            None,
        }
        
        // Start is called before the first frame update
        void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _player = GameObject.FindGameObjectWithTag("Player");
            _weaponController = GetComponentInChildren<WeaponController>();
            _weaponController.SetDamage(1);
            _targetable = GetComponent<Targetable>();
            _targetable.InitHealth(2, 2);
            SetState(State.Patrol);
        }

        // Update is called once per frame
        void Update()
        {
            if (_agent.pathPending) return;

            if (_targetable.GetHealth() <= 0)
            {
                Destroy(gameObject);
            }
            
            switch (_state)
            {
                case State.Patrol:
                    HandlePatrol();
                    break;
                case State.Chase:
                    HandleChase();
                    break;
                case State.Knockback:
                    HandleKnockback();
                    break;
            }
        }

        private void HandlePatrol()
        {
            if (Vector3.Distance(_agent.transform.position, _player.transform.position) < _chaseDistance)
            {
                SetState(State.Chase);
                return;
            }
            
            if (waypoints.Length == 0)
            {
                Debug.LogError("No waypoints found");
                return;
            }

            if (_agent.remainingDistance > 0.5f) return;
            
            _currentWaypoint = (_currentWaypoint + 1) % waypoints.Length;
            var waypoint = waypoints[_currentWaypoint];
            _agent.SetDestination(waypoint.transform.position);
        }
        
        private void HandleChase()
        {
            if (Vector3.Distance(_agent.transform.position, _player.transform.position) > _chaseDistance)
            {
                SetState(State.Patrol);
                return;
            }
            
            _agent.SetDestination(_player.transform.position);
        }
        
        private void HandleKnockback()
        {
            StartCoroutine(Knockback());
            SetState(State.None);
        }

        private IEnumerator Knockback()
        {
            // move agent in opposite direction of player
            var playerToSelf = transform.position - _player.transform.position;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().AddForce(playerToSelf.normalized * 1.5f, ForceMode.Impulse);
            yield return new WaitForSeconds(1);
            GetComponent<Rigidbody>().isKinematic = true;
            SetState(State.Patrol);
        }
        
        private void SetState(State state)
        {
            switch (state)
            {
                case State.Patrol:
                    if (_currentWaypoint >= waypoints.Length)
                    {
                        _currentWaypoint = waypoints.Length - 1;
                    } 
                    break;
                case State.Chase:
                    _weaponController.StartAttack();
                    break;
                case State.Knockback:
                    break;
            }
            _state = state;
        }
        
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                SetState(State.Knockback);
            }
        }
    }
}
