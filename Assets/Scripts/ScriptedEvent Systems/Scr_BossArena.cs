using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;

public class Scr_BossArena : MonoBehaviour
{
    [Header("Internal Components")]
    public GameObject ArenaCanvas;
    [SerializeField] private TMP_Text CurrentTimeText;

    Animator m_Animator;

    [SerializeField] List<GameObject> AI_Prefabs;

    [SerializeField] List<GameObject> InstantiatedAI;

    [Header("Boss Arena Properties")]
    [SerializeField] List<Transform> AI_SpawnLocations;

    public int SpawnCount = 18;
   
    public bool isBossFightActive = false;
    public UnityEvent OnBossFightComplete;

    bool m_doCompleteEventOnce = false;

    float BossArenaCurrentTime = 0.0f;
    [SerializeField] float BossArenaMaxTime = 60.0f;


    private string GetMinutesSecondsText(float FinishTime)
    {
        float minutes = Mathf.RoundToInt(FinishTime / 60);
        float seconds = Mathf.RoundToInt(FinishTime % 60);

        string minuteText = null;
        string secondsText = null;

        if (minutes < 10)
        {
            minuteText = "0" + minutes.ToString();
        }
        else
        {
            minuteText = Mathf.RoundToInt(minutes).ToString();
        }

        if (seconds < 10)
        {
            secondsText = "0" + Mathf.RoundToInt(seconds).ToString();
        }
        else
        {
            secondsText = Mathf.RoundToInt(seconds).ToString();
        }
        string finaltext = (minuteText + ":" + secondsText);
        return finaltext;
    }

    private IEnumerator SpawnAICoroutine()
    {
        for (int i = 0; i < SpawnCount; i++)
        {
            int k = Random.Range(0, AI_Prefabs.Count);
            int j = Random.Range(0, AI_SpawnLocations.Count);
            InstantiatedAI.Add(Instantiate(AI_Prefabs[k], AI_SpawnLocations[j], false));
            yield return new WaitForSeconds(1.0f);
        }

    }

    void SpawnAI()
    {
        //Initalise random AI Index and Random Position Index
        int k = Random.Range(0, AI_Prefabs.Count);
        int j = Random.Range(0, AI_SpawnLocations.Count);
        InstantiatedAI.Add(Instantiate(AI_Prefabs[k], AI_SpawnLocations[j], false));
    }
    private void CheckForMissingAI()
    {
        if (InstantiatedAI.Count > 0)
        {
            for (int i = 0; i < SpawnCount; i++)
            {
                if (InstantiatedAI[i] == null)
                {
                    InstantiatedAI.RemoveAt(i);
                }
            }
        }
    }

    void UpdateArenaCanvas()
    {
        CurrentTimeText.text = GetMinutesSecondsText(BossArenaCurrentTime);


    }
    public void StartSpawningAI()
    {
        StartCoroutine(SpawnAICoroutine());
    }

    // Enable The Canvas and enable the boss AI
    public void EnableBossFight()
    {
        //Enable Arena Canvas
        ArenaCanvas.SetActive(true);
        // Start Animator triggers and Spawn Ai Coroutine
        StartCoroutine(SpawnAICoroutine());
        m_Animator.SetTrigger("Lasers");
     
        //Enable Boss Canvas Fadein
        m_Animator.SetTrigger("FadeIn");

        //Set Boss fight active
        isBossFightActive = true;

        BossArenaCurrentTime = BossArenaMaxTime;
    }

    // Disable Boss AI and Canvas
    public void DisableBossFight()
    {
        ArenaCanvas.SetActive(false);
        isBossFightActive = false;
    }

    // Start is called before the first frame update
    void Start()
    {
       
        m_Animator = ArenaCanvas.GetComponent<Animator>();

       

    }
    void Update()
    {
        if (isBossFightActive)
        {
            BossArenaCurrentTime -= Time.deltaTime;

            CheckForMissingAI();
            UpdateArenaCanvas();

            if (InstantiatedAI.Count < 7)
            {
                SpawnAI();
            }

            if (!m_doCompleteEventOnce && BossArenaCurrentTime <= 0)
            {

                OnBossFightComplete.Invoke();
                DisableBossFight();

                m_doCompleteEventOnce = true;
                isBossFightActive = false;
            }
        }
       
        
        
    }

}
