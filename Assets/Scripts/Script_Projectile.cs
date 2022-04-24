using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Projectile : MonoBehaviour
{
    public float Damage = 10f;
    
    void RayCast(){
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 5))
        {
            var hitCollider = hit.collider.gameObject.GetComponent<CustomCollider>();
            if (hitCollider != null)
            {
                hitCollider.TakeDamage(Damage, hitCollider.damageType);
                Destroy(gameObject);
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        RayCast();
    }

    void OnTriggerEnter(Collider other)
    {
        var hitCollider = other.gameObject.GetComponent<CustomCollider>();
        if (hitCollider != null)
        {
            hitCollider.TakeDamage(Damage, hitCollider.damageType);
            Destroy(gameObject);
        }
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * 1f);
    }
}
