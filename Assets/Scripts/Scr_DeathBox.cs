using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_DeathBox : MonoBehaviour
{
    [SerializeField] Transform RespwanPositon;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Scr_PlayerHealth>().TakeDamage(50,0,0);
            other.transform.position = RespwanPositon.position;
        }
    }
}
