using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_CheckPointManager : MonoBehaviour
{
    public static Scr_CheckPointManager i;

    public Transform CurrentCheckPoint;
    public GameObject Player;
    private void Awake()
    {
        if (i == null)
        {
            i = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        CurrentCheckPoint = Player.transform;
    }

    public void RespawnPlayer()
    {
        Debug.Log("RespawningPlayer");
        Player.transform.position = CurrentCheckPoint.position;
    }

    public void SetCurrentCheckPoint(GameObject _Gameobject)
    {
        CurrentCheckPoint = _Gameobject.transform;
    }
}
