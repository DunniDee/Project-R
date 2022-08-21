using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_EnemyProjectile : MonoBehaviour
{
    public float m_fDamage = 0.0f;
    public float m_Lifetime = 0.0f;
    float initalY;
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision.name);
        if (collision.tag == "Player")
        {
            collision.GetComponent<Scr_PlayerHealth>().TakeDamage(m_fDamage, 0.25f, 0.3f);
            Debug.Log("Damage Done " + m_fDamage);
            Disable();
        }

        if (collision.tag == "Ground" || collision.tag == "Wallrunable")
        {
            Disable();
        }
    }
    private void Start()
    {
        initalY = transform.position.y;
    }
    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3(transform.position.x, initalY, transform.position.z);
        m_Lifetime -= Time.deltaTime;
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
