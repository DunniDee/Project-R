using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_SeekingRCProjectile : Script_RCProjectile
{
    [SerializeField] float SeekSpeed;


    TrailRenderer Trail;
    Transform SeekPos;
    
    private void Start() 
    {
        Trail = gameObject.GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 NextPos = transform.position + transform.forward * Speed * Time.deltaTime;

        if (SeekPos == null)
        {
            RaycastHit[] Hits;
            Hits = Physics.SphereCastAll(NextPos,5,transform.forward);

            foreach (var _hit in Hits)
            {
                if (_hit.transform.GetComponent<CustomCollider>())
                {
                    SeekPos = _hit.transform;
                    return;
                }
            }
        }
        else
        {
            NextPos =  Vector3.MoveTowards(NextPos,SeekPos.position, SeekSpeed * Time.deltaTime);   
        }

        float distance = (transform.position - NextPos).magnitude;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, Speed * Time.deltaTime))
        {
            var hitCollider = hit.collider.gameObject.GetComponent<CustomCollider>();
            if (hitCollider != null)
            {
                hitCollider.TakeDamage(Damage, hitCollider.damageType);
            }
            Disable();
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

    new public void SetlifeTime(float time)
    {
        Lifetime = time;
    }

    new public void SetDamage(float damage)
    {
        Damage = damage;
    }

    new public void SetSpeed(float speed)
    {
        Speed = speed;
    }

    new public void Disable()
    {
        SeekPos = null;
        Trail.Clear();
        ObjectPooler.Instance.ReturnObject(this.gameObject);
    }
}
