using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Projectile : MonoBehaviour
{
    public float Damage = 10f;

    void RayCast(){
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 1f,LayerMask.NameToLayer("Enemy"))){
            hit.collider.gameObject.GetComponent<CustomCollider>().TakeDamage(Damage, CustomCollider.DamageType.Critical);
            
        }
    }
    // Update is called once per frame
    void Update()
    {
        RayCast();
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 1f);
    }
}
