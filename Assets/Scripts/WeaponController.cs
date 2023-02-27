using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public bool isAttacking;

    public void setIsAttacking(bool value) {
        isAttacking = value;
    }
}