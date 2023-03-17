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

        target.OnHit(damage);
        hasHit = true;
    }
}