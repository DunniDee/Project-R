using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_TPGrenade : MonoBehaviour
{
    [SerializeField] GameObject Player;
    public ParticleSystem teleportFX;

    public void SetPlayer(GameObject player)
    {
        Player = player;
    }
    private void OnCollisionEnter(Collision other) 
    {
        Player.transform.position = transform.position;
        teleportFX.Play();
        Destroy(gameObject);
        
    }
}
