using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Acid : MonoBehaviour
{
    public float daamgePerTick = 0.5f;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var health = other.GetComponent<Scr_PlayerHealth>();

            health.TakeDamage(daamgePerTick,0.15f,0.1f);
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            var health = other.GetComponent<Scr_PlayerHealth>();

            health.TakeDamage(daamgePerTick, 0.15f, 0.1f);
        }
    }

}
