using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity​Engine.Rendering.HighDefinition;


public class Script_RCProjectile : MonoBehaviour
{
    [SerializeField] protected float Lifetime;
    [SerializeField] protected float Speed;
    [SerializeField] protected float Damage;
    [SerializeField] protected GameObject BulletHoleDecal;

    TrailRenderer m_Trail;

    private void Start() 
    {
        m_Trail = gameObject.GetComponentInChildren<TrailRenderer>();
    }

    /// <summary>
    /// translates the projectile , raycasts between the old position and the translated position and checks for collisions
    /// </summary>
    void Update()
    {
        Vector3 NextPos = transform.position + transform.forward * Speed * Time.deltaTime;

        float distance = (transform.position - NextPos).magnitude;

        RaycastHit hit;
        LayerMask AllMask = 1|2|3|4|5|6|7|8|9|10|11|12|13|14|15|16;
        if(Physics.Raycast(transform.position, transform.forward, out hit, Speed * Time.deltaTime, AllMask, QueryTriggerInteraction.Ignore))
        {
            GameObject Decal = ObjectPooler.Instance.GetObject(BulletHoleDecal);
            Decal.transform.position  = hit.point - hit.normal * 0.05f;
            Decal.transform.LookAt (hit.point + hit.normal);
            Decal.transform.localScale = new Vector3(Random.Range(0.5f, 1),1,Random.Range(0.5f, 1));
            Decal.transform.localRotation *= Quaternion.Euler(0,0,Random.Range(0, 360));

            Decal.GetComponent<DecalProjector>().fadeFactor = 1;

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

    /// <summary>
    /// Setter function for lifetime
    /// </summary>
    public void SetlifeTime(float time)
    {
        Lifetime = time;
    }

    /// <summary>
    /// Setter function for damage
    /// </summary>
    public void SetDamage(float damage)
    {
        Damage = damage;
    }

    /// <summary>
    /// set function for speed
    /// </summary>
    public void SetSpeed(float speed)
    {
        Speed = speed;
    }

    /// <summary>
    /// returns the projectile back to the object pooler
    /// </summary>
    public void Disable()
    {
        if (m_Trail != null)
        {
            m_Trail.Clear();
        }
        ObjectPooler.Instance.ReturnObject(this.gameObject);
    }
}
