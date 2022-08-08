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
            GameObject Enemy = SpawnPoint.SpawnEnemy();
            LocalEnemies.Add(Enemy);
            /*Enemy.GetComponent<Script_BaseAI>().WakeUpDisabled();*/
        }
    }

    public void DisableRoom()
    {
        LevelToggle.SetActive(false);
    }

    public void EnabeRoom()
    {
        LevelToggle.SetActive(true);

        SpawnEnemies();
    }

    public bool CheckRoomClear()
    {
        bool IsClear = true;
        if (LocalEnemies.Count > 0)
        {
            foreach (var Enemy in LocalEnemies)
            {
                if (Enemy.activeInHierarchy)
                {
                    IsClear = false;
                }
            }
        }
       

        return IsClear;
    }
}
