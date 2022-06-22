using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_RCProjectile : MonoBehaviour
{
    [SerializeField] protected float Lifetime;
    [SerializeField] protected float Speed;
    [SerializeField] protected float Damage;
    [SerializeField] protected GameObject BulletHoleDecal;

    TrailRenderer Trail;

    private void Start() 
    {
        Trail = gameObject.GetComponentInChildren<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 NextPos = transform.position + transform.forward * Speed * Time.deltaTime;

        float distance = (transform.position - NextPos).magnitude;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, Speed * Time.deltaTime))
        {
            GameObject Decal = ObjectPooler.Instance.GetObject(BulletHoleDecal);
            Decal.transform.position  = hit.point - hit.normal * 0.05f;
            Decal.transform.LookAt (hit.point + hit.normal);
            Decal.transform.localScale = new Vector3(Random.Range(0.5f, 1),1,Random.Range(0.5f, 1));
            Decal.transform.localRotation *= Quaternion.Euler(0,0,Random.Range(0, 360));

            var hitCollider = hit.collider.gameObject.GetComponent<CustomCollider>();
            if (hitCollider != null)
            {
                hitCollider.TakeDamage(Damage, hitCollider.damageType, transform.forward);
            }
            Disable();
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
        Trail.Clear();
        ObjectPooler.Instance.ReturnObject(this.gameObject);
    }
}
