using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class scr_BoilerRoom : MonoBehaviour
{
    [Header("Internal Components")]
    public GameObject ArenaCanvas;
    public TMP_Text Timer;
    public UnityEvent OnRoomCompletedEvent;

    public List<Transform> SpawnPosition;

    public float MaxTime = 60;
    public float currentTime = 0;

    private int maxAmountEnemies = 5;

    [SerializeField] List<GameObject> AI_Prefabs;

    [SerializeField] List<GameObject> InstantiatedAI;
    bool isArenaActive = false;

    private IEnumerator SpawnAICoroutine()
    {
        int EnemiesToSpawn = maxAmountEnemies - InstantiatedAI.Count;
        for (int i = 0; i < EnemiesToSpawn; i++)
        {
            int k = Random.Range(0, AI_Prefabs.Count);
            int j = Random.Range(0, SpawnPosition.Count);
            InstantiatedAI.Add(Instantiate(AI_Prefabs[k], SpawnPosition[j], false));
            yield return new WaitForSeconds(1.0f);
        }
    }

    private void UpdateTimer()
    {
        currentTime -= Time.deltaTime;
        Timer.text = "Time Till Explosion 00:" + currentTime.ToString("F0");
    }
    public void StartFinalEvent()
    {
        ArenaCanvas.SetActive(true);
        StartCoroutine(SpawnAICoroutine());
        isArenaActive = true;
        currentTime = MaxTime;
    }

    public void FinishFinalEvent()
    {
        ArenaCanvas.SetActive(false);
        //StartCoroutine(SpawnAICoroutine());
        isArenaActive = false;
    }

    void Update()
    {
        if(isArenaActive)
        {
            
            UpdateTimer();
            if (InstantiatedAI.Count < maxAmountEnemies)
            {
                SpawnAICoroutine();
            }

            if (currentTime <= 0)
            {
                FinishFinalEvent();
            }
        }
    }
}
