using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider c) {
        WeaponController weapon = c.gameObject.GetComponent<WeaponController>();
        if(weapon != null){
            if(c.gameObject.tag == "Weapon" && weapon.isAttacking == true) {
                Destroy(this.gameObject);
            }
        }
    }
}
