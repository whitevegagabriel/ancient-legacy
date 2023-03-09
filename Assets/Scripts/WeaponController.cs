using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public bool isAttacking;

    public void setIsAttacking(bool value) {
        isAttacking = value;
    }

    public void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<Targetable>();
        if (target != null && isAttacking)
        {
            target.OnHit();
        }
    }
}