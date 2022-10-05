﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity​Engine.Rendering.HighDefinition;
// public class Scr_ExplosiveBarrel : Scr_DesctruableObject
// {
//     [Header("Explosive Properties")]
//     public float ExplosionForce = 100.0f;
//     public float ExplosionRadius = 2.0f;
//     public LayerMask DamageableMask;
//     public bool ShowGizmos = true;
//     [Header("Prefabs")]
//     public GameObject ExplosionVFX;
//     public GameObject ExplosionDecal;
//     [Header("Object Properties")]
//     public float LifeTime = 5.0f;
//     public void OnDeathEvent()
//     {
//         Instantiate(ExplosionVFX, transform);
//         SpawnExplosionDecal();
//         RaycastHit[] objectsInRange = Physics.SphereCastAll(transform.position, ExplosionRadius,transform.forward, 0.1f, DamageableMask);
//         Debug.Log(objectsInRange.Length);
//         foreach (RaycastHit hit in objectsInRange)
//         {
//             hit.rigidbody.AddForce(-(transform.position - hit.transform.position) * ExplosionForce, ForceMode.Impulse);
            
//         }
//         Destroy(gameObject, LifeTime);
//     }
//     public void SpawnExplosionDecal()
//     {
//         RaycastHit hit;
//         if (Physics.Raycast(transform.position + Vector3.up, -transform.up, out hit))
//         {
//             GameObject Decal = ObjectPooler.Instance.GetObject(ExplosionDecal);
//             Decal.transform.localScale = new Vector3(5, 5, 1);
//             Decal.transform.position = hit.point - hit.normal * 0.05f;
//             Decal.transform.LookAt(hit.point + hit.normal);
//             Decal.transform.localScale = new Vector3(Random.Range(0.5f, 1), 1, Random.Range(0.5f, 1));
//             Decal.transform.localRotation *= Quaternion.Euler(0, 0, Random.Range(0, 360));

//             Decal.GetComponent<DecalProjector>().fadeFactor = 1;

            

//            // Disable();
//         }
//     }
//     public void Disable()
//     {
//         ObjectPooler.Instance.ReturnObject(this.gameObject);
//     }
//     // Start is called before the first frame update
//     void Start()
//     {
//         base.Start();
//         OnDestructEvent += OnDeathEvent;
//     }

//     // Update is called once per frame
//     void Update()
//     {
       
//     }

//     public void OnDrawGizmosSelected()
//     {
//         Gizmos.color = Color.red;
//         Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
//     }
// }

public class Scr_ExplosiveBarrel : MonoBehaviour, IDamageable
{
    public float m_Health;
    public float ExplosionRadius;

    bool HasExploded = false;

    [SerializeField] GameObject ExplosionPrefab;

    private void Explode()
    {
        if (HasExploded)
        {
            return;
        }
        HasExploded = true;
        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, ExplosionRadius);
        List<Transform> TransfromList = new List<Transform>();
        foreach (var hitCollider in hitColliders)
        {
            var CustomCollider = hitCollider.gameObject.GetComponent<CustomCollider>();
            if (CustomCollider != null)
            {
                if (!TransfromList.Contains(CustomCollider.transform.root))
                {  
                    CustomCollider.TakeDamage(100, CustomCollider.DamageType.Normal, -transform.forward);
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

        Instantiate(ExplosionPrefab,transform.position,transform.rotation);
        Destroy(gameObject,0.25f);
    }
    
    public bool Damage(float _Damage, CustomCollider.DamageType _DamageType,Vector3 _direction)
    {
        m_Health -= _Damage;

        if (m_Health <= 0)
        {
            Explode();
            return true;
        }
        else
        {
            return false;
        }
    }

}
