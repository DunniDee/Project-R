using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_TPGrenade : MonoBehaviour
{
    [SerializeField] GameObject Player;
   [SerializeField] GameObject teleportFX;

    public void SetPlayer(GameObject player)
    {
        Player = player;
    }
    private void OnCollisionEnter(Collision other) 
    {
        Player.transform.position = transform.position;
        GameObject VFX = ObjectPooler.Instance.GetObject(teleportFX);
        VFX.transform.position = transform.position;
        Destroy(gameObject);
        
    }
}
