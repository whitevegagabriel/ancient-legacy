using Combat;
using UnityEngine;
using UnityEngine.Events;
using AI;

public class WeaponController : MonoBehaviour
{
    public bool isAttacking;
    private bool hasHit;
    private int damage;

    public void SetDamage(int newDamage) {
        damage = newDamage;
    }

    public void StartAttack() {
        isAttacking = true;
        hasHit = false;
    }
    
    public void StopAttack() {
        isAttacking = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<Targetable>();
        if (other.gameObject.layer == gameObject.layer || target == null || hasHit || !isAttacking) return;
        if (other.gameObject.CompareTag("Player")) {
            if (!other.gameObject.GetComponent<PlayerController>().isBlocking) {
                target.OnHit(damage);
            }
            else {
                EventManager.TriggerEvent<BlockEvent, Vector3>(other.transform.position);
            }
        }
        else if (other.gameObject.CompareTag("Skeleton"))
        {
            if (!other.gameObject.GetComponent<SkeletonAI>().isBlocking)
            {
                target.OnHit(damage);
            }
            else
            {
                EventManager.TriggerEvent<BlockEvent, Vector3>(other.gameObject.transform.position);
            }
        }
        else {
            target.OnHit(damage);
        }
        hasHit = true;
    }
}