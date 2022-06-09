using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_ShipUIController : MonoBehaviour
{
    public GameObject BoardShipMessage;

    public void EnableBoardMessage(bool _bool)
    {
        BoardShipMessage.SetActive(_bool);
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(Scr_EnemyShip enemyShip in GameObject.FindObjectsOfType<Scr_EnemyShip>())
        {
            enemyShip.onShipDetectedEvent += EnableBoardMessage;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
