using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_SalvoMissile : Scr_RCSplashProjectile
{
    [SerializeField] float m_Wavyness;
    new protected void Update()
    {
        Vector3 NextPos = transform.position + transform.forward  * Speed * Time.deltaTime + transform.up * Random.Range(-m_Wavyness,m_Wavyness) * Mathf.Sin(Lifetime * 10) * Time.deltaTime  +  transform.right * Random.Range(-m_Wavyness,m_Wavyness) * Mathf.Cos(Lifetime * 10) * Time.deltaTime;

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
            Disable();
        }
    }
}
