using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class RadialDamageController : MonoBehaviour
    {
        private const float StartRadius = 0.1f;
        private const float FinalRadius = 15;
        private const float RadiusIncrease = 0.1f;
        private CharacterController _playerController;
        private LineRenderer _lineRenderer;
        private float _currentRadius;
        private bool _alreadyCollided;
        
        private void Start()
        {
            _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
            // TODO: switch to a particle-system-based solution instead of using LineRenderer
            _lineRenderer = GetComponent<LineRenderer>();
            _currentRadius = StartRadius;
        }

        private void FixedUpdate()
        {
            Draw(_currentRadius);

            if (CollidingWithPlayer(_currentRadius) && !_alreadyCollided)
            {
                _alreadyCollided = true;
                var playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
                playerHealth.DecreaseHealth(2);
            }

            _currentRadius += RadiusIncrease;

            // destroy if max radius is reached
            if (_currentRadius >= FinalRadius)
            {
                Destroy(transform.parent.gameObject);
            }
        }

        private bool CollidingWithPlayer(float radiusFromCenter)
        {
            // check if player within error margin of collision
            var distance = Vector3.Distance(_playerController.transform.position,  transform.parent.position);
            var jumpHeight =_playerController.transform.position.y - transform.parent.position.y;
            return Mathf.Abs(distance - radiusFromCenter) < .1 && Mathf.Abs(jumpHeight) < .1;
        }
        
        void Draw(float radius) // only need to draw when something changes
        {
            List<Vector3> points = new List<Vector3>();
            int numPoints = 100; // number of points on the circumference

            for (int i = 0; i < numPoints; i++)
            {
                float angle = i * Mathf.PI * 2 / numPoints;
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;
                points.Add(new Vector3(x, 0, z));
            }

            _lineRenderer.positionCount = points.Count;
            _lineRenderer.SetPositions(points.ToArray());
        }
    }
}
