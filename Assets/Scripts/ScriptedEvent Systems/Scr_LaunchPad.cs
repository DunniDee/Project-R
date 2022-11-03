using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_LaunchPad : MonoBehaviour
{
    [SerializeField] float Force;
    [SerializeField] float Height;

    [SerializeField] AudioSource AS;
    [SerializeField] AudioClip BoostSound;
    private void OnTriggerEnter(Collider other) 
    {
        Debug.Log(other);
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Scr_PlayerMotor>().Launch(transform.forward * Force, Height);
            AS.PlayOneShot(BoostSound);
        }
    }
}
