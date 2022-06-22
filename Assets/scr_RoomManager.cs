using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_RoomManager : MonoBehaviour
{
    [SerializeField] Scr_EnemySpawnPoint[] LocalSpawnPoints;
    [SerializeField] List<GameObject> LocalEnemies;

    private void Start() 
    {
        LocalSpawnPoints = GetComponentsInChildren<Scr_EnemySpawnPoint>();
    }

    void SpawnEnemies()
    {
        foreach (var SpawnPoint in LocalSpawnPoints)
        {
            LocalEnemies.Add(SpawnPoint.SpawnEnemy());
        }
    }
}
