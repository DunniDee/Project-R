using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_EnemyProjectile : MonoBehaviour
{
    public float m_fDamage = 0.0f;
    public float m_Lifetime = 0.0f;
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<Scr_PlayerHealth>().TakeDamage(m_fDamage);
            Debug.Log("Damage Done " + m_fDamage);
            //Destroy(gameObject);
            Disable();
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        m_Lifetime-= Time.deltaTime;
        if (m_Lifetime < 0)
        {
            Disable();
        }
    }

    public void Disable()
    {
        m_Lifetime = 3;
        ObjectPooler.Instance.ReturnObject(this.gameObject);
    }
}
