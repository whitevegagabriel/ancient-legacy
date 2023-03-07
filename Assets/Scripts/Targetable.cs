using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetable : MonoBehaviour
{
    private Animator animator;
    float cooldown;
    float nextHitTime;
    AI.BossAI bossAI;
    void Start()
    {
        animator = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>();
        cooldown = animator.GetCurrentAnimatorStateInfo(0).length;
        nextHitTime = Time.time;
        bossAI = GameObject.FindGameObjectWithTag("Boss").GetComponent<AI.BossAI>();
    }


    void OnTriggerEnter(Collider c) {
        WeaponController weapon = c.gameObject.GetComponent<WeaponController>();
        if(weapon != null){
            Debug.Log(weapon.isAttacking);
            if(c.gameObject.tag == "Weapon" && weapon.isAttacking == true && Time.time >= nextHitTime) {
                bossAI.DecreaseHealth(2);
                nextHitTime = Time.time + cooldown;
            }
        }
    }
}