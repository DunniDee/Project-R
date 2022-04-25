using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_RCProjectile : MonoBehaviour
{
    [SerializeField] float Speed;
    [SerializeField] float Damage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 NextPos = transform.position + transform.forward * Speed * Time.deltaTime;

        float distance = (transform.position - NextPos).magnitude;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, Speed * Time.deltaTime))
        {
            var hitCollider = hit.collider.gameObject.GetComponent<CustomCollider>();
            if (hitCollider != null)
            {
                hitCollider.TakeDamage(Damage, hitCollider.damageType);
            }
            Destroy(gameObject);
        }

        Debug.DrawLine(transform.position, NextPos, Color.red);

        transform.position = NextPos;
    }
}
