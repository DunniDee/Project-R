using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Acid : MonoBehaviour
{
    public float Damage = 10.0f;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            var health = other.GetComponent<Scr_PlayerHealth>();
            var motor = other.GetComponent<Scr_PlayerMotor>();

            health.TakeDamage(Damage);
            motor.m_MomentumDirection = new Vector3(motor.m_MomentumDirection.x, 10, motor.m_MomentumDirection.z);
        }
    }
   
}
