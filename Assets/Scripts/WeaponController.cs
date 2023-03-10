using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public bool isAttacking;
    private bool hasHit;

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
        if (target == null || hasHit || !isAttacking) return;

        Debug.Log("Hit " + other.name);
        target.OnHit();
        hasHit = true;
    }
}