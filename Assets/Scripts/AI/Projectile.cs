using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class Projectile : MonoBehaviour
{
    private GameObject _player;
    private WeaponController _weaponController;
    private SkeletonProjectileAI _skeletonUser;
    private int frames;
    private bool frameCount;
    private Renderer rend;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _weaponController = GetComponent<WeaponController>();
        _weaponController.SetDamage(1);
        rend = GetComponent<Renderer>();
        frames = 0;
        frameCount = false;
        _weaponController.StartAttack();
    }

    void Update()
    {
        if (frameCount)
        {
            frames = frames + 1;
            if (frames >= 20)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "ProjectileUser")
        {
            if (other.gameObject.tag == "Player")
            {
                rend.enabled = false;
                frameCount = true;
            }
            else if (other.gameObject.tag == "Wall")
            {
                gameObject.SetActive(false);
            }
        }
    }
}
