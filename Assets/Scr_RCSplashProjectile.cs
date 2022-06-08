using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_RCSplashProjectile : Script_RCProjectile
{
    [SerializeField] protected float ExplosionRadius;

    [SerializeField] protected GameObject Explosion;
    new protected void Update()
    {
        Vector3 NextPos = transform.position + transform.forward  * Speed * Time.deltaTime;

        float distance = (transform.position - NextPos).magnitude;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, Speed * Time.deltaTime))
        {
            Hit();

            //Bounce
            //transform.LookAt(transform.position + Vector3.Reflect(hit.point - transform.position, hit.normal));
        }

        Debug.DrawLine(transform.position, NextPos, Color.red);

        transform.LookAt(NextPos);
        transform.position = NextPos;

        if (Lifetime > 0)
        {
            Lifetime-= Time.deltaTime;
        }
        else
        {
            GameObject Expl = ObjectPooler.Instance.GetObject(Explosion);
            Expl.GetComponent<Scr_PAExplosion>().setRadius(ExplosionRadius);
            Expl.transform.position = transform.position;
            Disable();
        }
    }

    protected private void Hit() 
    {
        GameObject Expl = ObjectPooler.Instance.GetObject(Explosion);
        Expl.GetComponent<Scr_PAExplosion>().setRadius(ExplosionRadius);
        Expl.transform.position = transform.position;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, ExplosionRadius);
        List<Transform> TransfromList = new List<Transform>();
        foreach (var hitCollider in hitColliders)
        {
            var CustomCollider = hitCollider.gameObject.GetComponent<CustomCollider>();
            if (CustomCollider != null)
            {
                if (!TransfromList.Contains(CustomCollider.transform.root))
                {  
                    CustomCollider.TakeDamage(Damage, CustomCollider.DamageType.Normal);
                    TransfromList.Add(CustomCollider.transform.root);
                }

                // if (CustomCollider.damageType == CustomCollider.DamageType.Critical)
                // {
                //     CustomCollider.TakeDamage(Damage, CustomCollider.DamageType.Normal);
                // }
            }
        }

        Disable();
    }

    public void SetRadius(float _Radius)
    {
        ExplosionRadius = _Radius;
    }
}
