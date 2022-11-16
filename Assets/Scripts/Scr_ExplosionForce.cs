using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_ExplosionForce : MonoBehaviour
{
    [SerializeField] float ExplosionRadius;
    [SerializeField] LayerMask DamageableMask;
    [SerializeField] float ExplosionForce;
    private void OnEnable() 
    {
        RaycastHit[] objectsInRange = Physics.SphereCastAll(transform.position, ExplosionRadius,transform.forward, ExplosionRadius, DamageableMask);
        Debug.Log(objectsInRange.Length);
        foreach (RaycastHit hit in objectsInRange)
        {   
            if (hit.transform.GetComponent<Scr_PlayerMotor>())
            {
                Debug.Log("Player in Range");
            }

            hit.rigidbody.AddForce(-(transform.position - hit.transform.position) * ExplosionForce, ForceMode.Impulse);
        } 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
    }
}
