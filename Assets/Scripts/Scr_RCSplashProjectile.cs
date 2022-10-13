using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_RCSplashProjectile : Script_RCProjectile
{
    [SerializeField] protected float ExplosionRadius;

    [SerializeField] protected GameObject Explosion;
    protected void Update()
    {
        Vector3 NextPos = transform.position + transform.forward  * Speed * Time.deltaTime;

        float distance = (transform.position - NextPos).magnitude;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, Speed * Time.deltaTime, layermask,QueryTriggerInteraction.Ignore))
        {
            Hit();
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
            // GameObject Expl = ObjectPooler.Instance.GetObject(Explosion);
            // Expl.GetComponent<Scr_PAExplosion>().setRadius(ExplosionRadius);
            // Expl.transform.position = transform.position;

            Hit();
        }
    }

    protected private void Hit() 
    {
        GameObject Expl = ObjectPooler.Instance.GetObject(Explosion);
        Expl.transform.position = transform.position;
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

            if (hitCollider.GetComponent<Script_InteractEvent>())
            {
                var hitEvent = hitCollider.GetComponent<Script_InteractEvent>();
                if (hitEvent.EventType == Script_InteractEvent.InteractEventType.OnHit)
                {
                    hitEvent.Interact();
                }
            }
        }
        Transform playerTransform = FindObjectOfType<Scr_PlayerMotor>().transform;
        float sqrDist = (transform.position - playerTransform.position).sqrMagnitude;

        if (sqrDist < ExplosionRadius*ExplosionRadius)
        {
            Vector3 Dir = (transform.position - playerTransform.position).normalized;
            playerTransform.GetComponent<Scr_PlayerMotor>().ExplosionForce(-Dir * 10);
        }

        Disable();
    }

    public void SetRadius(float _Radius)
    {
        ExplosionRadius = _Radius;
    }
}
