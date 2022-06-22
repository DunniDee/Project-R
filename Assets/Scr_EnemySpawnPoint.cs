using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_EnemySpawnPoint : MonoBehaviour
{
    public GameObject Enemy;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        DisableEnemy();
    }

    public void EnableEnemy() 
    {
        Enemy.SetActive(true);
    }

    public void DisableEnemy()
    {
        Debug.Log("Wah");
        Enemy.SetActive(false);
    }
}
