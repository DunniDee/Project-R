using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_EnemySpawnPoint : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject SpawnedEnemy;


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        //SpawnEnemy();
    }

    public GameObject SpawnEnemy()
    {
        SpawnedEnemy = ObjectPooler.Instance.GetObject(Enemy);
        SpawnedEnemy.transform.position = transform.position;
        SpawnedEnemy.transform.rotation = transform.rotation;

        return SpawnedEnemy;
    }
}