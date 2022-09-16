using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_CheckPoint : MonoBehaviour
{
    [SerializeField] AudioSource AS;

    private void OnTriggerEnter(Collider other) 
    {
        AS.Play();
        Script_SceneManager.Instance.SetSpawnPoint(transform);
    }
}
