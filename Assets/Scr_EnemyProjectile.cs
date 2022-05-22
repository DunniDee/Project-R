using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_EnemyProjectile : MonoBehaviour
{
    public float m_fDamage = 0.0f;
    void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<Scr_PlayerHealth>().TakeDamage(m_fDamage);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
