using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class scr_BoilerRoom : MonoBehaviour
{
    [Header("Internal Components")]
    public Animator UIAnimator;
    public GameObject ArenaCanvas;
    public TMP_Text Timer;
    public UnityEvent OnRoomCompletedEvent;

    public List<Transform> SpawnPosition;

    public float MaxTime = 60;
    public float currentTime = 0;

    [SerializeField] float SpawnInterval = 2.5f;
    float SpawnTimer = 0.0f;

    private int maxAmountEnemies = 5;

    [SerializeField] List<GameObject> AI_Prefabs;

    [SerializeField] List<GameObject> InstantiatedAI;
    [SerializeField] bool isArenaActive = false;

    private int PressedButtons = 0 ;
    [SerializeField] int MaxButtons = 3;
    public void IncreaseButtonCount() { PressedButtons++; }
    public void Start()
    {
        
    }

    private IEnumerator SpawnAICoroutine()
    {
        int EnemiesToSpawn = maxAmountEnemies - InstantiatedAI.Count;
        for (int i = 0; i < EnemiesToSpawn; i++)
        {
      
            yield return new WaitForSeconds(1.0f);
        }
    }

    private void UpdateInstancedAI() {
        int i = 0;
        foreach (GameObject AI in InstantiatedAI)
        {
            i++;
            if (AI == null)
            {
                InstantiatedAI.RemoveAt(i);
            }
        }
    }
    private void UpdateTimer()
    {
        SpawnTimer -= Time.deltaTime;
        if (SpawnTimer <= 0.0f)
        {
            SpawnAI();
            SpawnTimer = SpawnInterval;
        }
        //Update Time Till Explosion UI
        currentTime -= Time.deltaTime;
        Timer.text = "Time Till Explosion 00:" + currentTime.ToString("F0");
    }
    public void StartFinalEvent()
    {
        ArenaCanvas.SetActive(true);
        UIAnimator.SetTrigger("FadeIn");
        StartCoroutine(SpawnAICoroutine());
        SpawnTimer = SpawnInterval;
        isArenaActive = true;
        currentTime = MaxTime;
    }

    public void SpawnAI()
    {
       
        Instantiate(AI_Prefabs[Random.Range(0, AI_Prefabs.Count)], SpawnPosition[Random.Range(0, SpawnPosition.Count)], false);
    }
    public void FinishFinalEvent()
    {
        ArenaCanvas.SetActive(false);
        OnRoomCompletedEvent.Invoke();
        isArenaActive = false;
    }

    void Update()
    {
        if(isArenaActive)
        {
            UpdateInstancedAI();
            UpdateTimer();

            if (currentTime <= 0)
            {
                FinishFinalEvent();
            }
        }
    }
}
