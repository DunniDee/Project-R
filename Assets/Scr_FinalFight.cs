using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_FinalFight : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("BeginFinalFight");
        }    
    }
}
