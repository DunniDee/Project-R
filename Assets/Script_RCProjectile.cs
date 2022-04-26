using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_RCProjectile : MonoBehaviour
{
    [SerializeField] float Lifetime;
    [SerializeField] float Speed;
    [SerializeField] float Damage;

    TrailRenderer Trail;

    private void Start() 
    {
        Trail = gameObject.GetComponent<TrailRenderer>();
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
            gameObject.SetActive(false);
            //Bounce
            //transform.LookAt(transform.position + Vector3.Reflect(hit.point - transform.position, hit.normal));
        }

        Debug.DrawLine(transform.position, NextPos, Color.red);

        transform.position = NextPos;

        if (Lifetime > 0)
        {
            Lifetime-= Time.deltaTime;
        }
        else
        {
            Disable();
        }
    }

    public void SetlifeTime(float time)
    {
        Lifetime = time;
    }

    public void SetDamage(float damage)
    {
        Damage = damage;
    }

    public void SetSpeed(float speed)
    {
        Speed = speed;
    }

    public void Disable()
    {
        gameObject.SetActive(false);
        Trail.Clear();
    }
}
