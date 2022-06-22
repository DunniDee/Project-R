using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_RoomManager : MonoBehaviour
{
    [SerializeField] Scr_EnemySpawnPoint[] LocalSpawnPoints;
    [SerializeField] List<GameObject> LocalEnemies;
    [SerializeField] scr_Door Exit;

    [SerializeField] GameObject LevelToggle;

    private void Start() 
    {
        LocalSpawnPoints = GetComponentsInChildren<Scr_EnemySpawnPoint>();
        DisableRoom();
    }

    void SpawnEnemies()
    {
        foreach (var SpawnPoint in LocalSpawnPoints)
        {
            LocalEnemies.Add(SpawnPoint.SpawnEnemy());
        }
    }

    public void DisableRoom()
    {
        LevelToggle.SetActive(false);
    }

    public void EnabeRoom()
    {
        LevelToggle.SetActive(true);
    }
}
