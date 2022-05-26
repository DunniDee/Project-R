using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_SalvoMissile : Scr_RCSplashProjectile
{
    [SerializeField] float m_Wavyness;
    [SerializeField] Transform SeekTransform;

    public void SetSeekTransform(Transform _transform)
    {
        SeekTransform = _transform;
    }

    new protected void Update()
    {
        Vector3 NextPos;

        if (SeekTransform == null)
        {
            NextPos = transform.position + transform.forward  * Speed * Time.deltaTime
            + transform.up * Random.Range(-m_Wavyness,m_Wavyness) * Mathf.Sin(Lifetime * 10) * Time.deltaTime  
            + transform.right * Random.Range(-m_Wavyness,m_Wavyness) * Mathf.Cos(Lifetime * 10) * Time.deltaTime;    
        }
        else
        {
            Vector3 SeekDir = SeekTransform.position - transform.position;

            NextPos = transform.position + transform.forward  * Speed * Time.deltaTime
            + transform.up * Random.Range(-m_Wavyness,m_Wavyness) * Mathf.Sin(Lifetime * 10) * Time.deltaTime  
            + transform.right * Random.Range(-m_Wavyness,m_Wavyness) * Mathf.Cos(Lifetime * 10) * Time.deltaTime
            + SeekDir * 0.1f * Time.deltaTime;   
        }
        

        float distance = (transform.position - NextPos).magnitude;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, Speed * Time.deltaTime))
        {
            Hit();
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
}
