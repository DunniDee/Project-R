using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity​Engine.Rendering.HighDefinition;
public class Scr_ExplosiveBarrel : Scr_DesctruableObject
{
    [Header("Explosive Properties")]
    public float ExplosionForce = 100.0f;
    public float ExplosionRadius = 2.0f;
    public LayerMask DamageableMask;
    public bool ShowGizmos = true;
    [Header("Prefabs")]
    public GameObject ExplosionVFX;
    public GameObject ExplosionDecal;
    [Header("Object Properties")]
    public float LifeTime = 5.0f;
    public void OnDeathEvent()
    {
        Instantiate(ExplosionVFX, transform);
        SpawnExplosionDecal();
        RaycastHit[] objectsInRange = Physics.SphereCastAll(transform.position, ExplosionRadius,transform.forward, 0.1f, DamageableMask);
        Debug.Log(objectsInRange.Length);
        foreach (RaycastHit hit in objectsInRange)
        {
            hit.rigidbody.AddForce(-(transform.position - hit.transform.position) * ExplosionForce, ForceMode.Impulse);
            
        }
        Destroy(gameObject, LifeTime);
    }
    public void SpawnExplosionDecal()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, -transform.up, out hit))
        {
            GameObject Decal = ObjectPooler.Instance.GetObject(ExplosionDecal);
            Decal.transform.localScale = new Vector3(5, 5, 1);
            Decal.transform.position = hit.point - hit.normal * 0.05f;
            Decal.transform.LookAt(hit.point + hit.normal);
            Decal.transform.localScale = new Vector3(Random.Range(0.5f, 1), 1, Random.Range(0.5f, 1));
            Decal.transform.localRotation *= Quaternion.Euler(0, 0, Random.Range(0, 360));

            Decal.GetComponent<DecalProjector>().fadeFactor = 1;

            

           // Disable();
        }
    }
    public void Disable()
    {
        ObjectPooler.Instance.ReturnObject(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        OnDestructEvent += OnDeathEvent;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
    }
}
