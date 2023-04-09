using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class Projectile : MonoBehaviour
{
    private GameObject _player;
    private WeaponController _weaponController;
    private SkeletonProjectileAI _skeletonUser;
    private bool isTriggered = true;
    public float speed = 0.1F;
    private Vector3 position;
    private float ticks;
    private float maxTicks;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _weaponController = GetComponent<WeaponController>();
        _weaponController.SetDamage(1);
        ticks = 0;
        maxTicks = (1/speed);
        maxTicks = Mathf.Ceil(maxTicks);
        position = _player.transform.position;
    }

    void Update()
    {
        transform.position += (transform.position - position) * speed;
        ticks += 1;
    }
}
