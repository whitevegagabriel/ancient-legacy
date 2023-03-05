using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEditor.ProBuilder;
using UnityEngine;
using UnityEngine.ProBuilder;

namespace AI
{
    public class RadialDamageController : MonoBehaviour
    {
        private const float FinalRadius = 15;
        private ProBuilderMesh _proBuilderMesh;
        private Vector3 _center;
        private bool _alreadyCollided;

        // Start is called before the first frame update
        private void Start()
        {
            // TODO: switch to a particle-system-based solution instead of using ProBuilder
            _proBuilderMesh = GetComponent<ProBuilderMesh>();
            var vertices = _proBuilderMesh.GetVertices();
            _center = vertices.Aggregate(Vector3.zero, (current, vertex) => current + vertex.position) /
                      vertices.Length;
        }

        private void FixedUpdate()
        {
            var vertices = _proBuilderMesh.GetVertices();

            // increase radius for each vertex position
            var maxRadius = 0f;
            foreach (var vertex in vertices)
            {
                var planeCenter = new Vector3(_center.x, vertex.position.y, _center.z);
                var radius = Vector3.Distance(planeCenter, vertex.position);
                var newRadius = radius + 0.1f;
                if (newRadius > maxRadius)
                {
                    maxRadius = newRadius;
                }

                vertex.position = Vector3.LerpUnclamped(planeCenter, vertex.position, newRadius / radius);
            }

            _proBuilderMesh.SetVertices(vertices);
            _proBuilderMesh.ToMesh();
            _proBuilderMesh.Refresh();
            
            if (CollidingWithPlayer(maxRadius) && !_alreadyCollided)
            {
                _alreadyCollided = true;
                var playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
                playerHealth.DecreaseHealth(2);
            }

            // destroy if max radius is reached
            if (maxRadius >= FinalRadius)
            {
                Destroy(transform.parent.gameObject);
            }
        }

        private bool CollidingWithPlayer(float radiusFromCenter)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            // check if player within error margin of collision
            var distance = Vector3.Distance(player.transform.position, _center);
            return distance - radiusFromCenter < .1;
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.gameObject.name);
        }
    }
}
