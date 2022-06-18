using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_CommanderSphere : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.GetMask("Enemy"))
        {
            var agent = other.GetComponentInParent<Script_BaseAI>();
            agent.Config.projectileDamage += agent.Config.projectileDamage * 0.5f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.GetMask("Enemy"))
        {
            var agent = other.GetComponentInParent<Script_BaseAI>();
            agent.Config.projectileDamage -= agent.Config.projectileDamage * 0.5f;
        }
    }
  
}
