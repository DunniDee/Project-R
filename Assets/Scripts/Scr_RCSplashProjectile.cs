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
        if(Physics.Raycast(transform.position, transform.forward, out hit, Speed * Time.deltaTime, layermask,QueryTriggerInteraction.Ignore))
        {
            Hit();

            Debug.Log("Decal SPawning at " + hit.point);
            GameObject Decal = ObjectPooler.Instance.GetObject(BulletHoleDecal);
            Decal.transform.position  = hit.point - hit.normal * 0.05f;
            Decal.transform.LookAt (hit.point + hit.normal);
            Decal.transform.localScale = new Vector3(Random.Range(0.5f, 1),1,Random.Range(0.5f, 1));
            Decal.transform.localRotation *= Quaternion.Euler(0,0,Random.Range(0, 360));
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
        //Expl.GetComponent<Scr_PAExplosion>().setRadius(ExplosionRadius);
        //Expl.transform.position = transform.position;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, ExplosionRadius);
        List<Transform> TransfromList = new List<Transform>();
        foreach (var hitCollider in hitColliders)
        {
            var CustomCollider = hitCollider.gameObject.GetComponent<CustomCollider>();
            if (CustomCollider != null)
            {
                if (!TransfromList.Contains(CustomCollider.transform.root))
                {  
                    CustomCollider.TakeDamage(Damage, CustomCollider.DamageType.Normal, -transform.forward);
                    TransfromList.Add(CustomCollider.transform.root);
                }
            }
        }

        float sqrDist = (transform.position - Script_PlayerStatManager.Instance.PlayerTransform.position).sqrMagnitude;

        if (sqrDist < ExplosionRadius*ExplosionRadius)
        {
            Vector3 Dir = (transform.position - Script_PlayerStatManager.Instance.PlayerTransform.position).normalized;
            Script_PlayerStatManager.Instance.Motor.ExplosionForce(-Dir * 10);
        }

        Disable();
    }

    public void SetRadius(float _Radius)
    {
        ExplosionRadius = _Radius;
    }
}
