using Combat;
using UnityEngine;
using UnityEngine.Events;

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
        if (other.gameObject.tag == "Player") {
            if (!GameObject.Find("Player").GetComponent<PlayerController>().isBlocking) {
                PlayerController.health -= 1;
            }
            else {
                EventManager.TriggerEvent<BlockEvent, Vector3>(GameObject.Find("Player").transform.position);
            }
        }
        else {
            target.OnHit(damage);
        }
        hasHit = true;
    }
}