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
    Animator m_Animator;
    Slider m_BossHealthSlider;
    [SerializeField] List<GameObject> AI_Prefabs;

    [SerializeField] List<GameObject> InstantiatedAI;
    [Header("Boss Arena Properties")]
    [SerializeField] List<AI_Brute> AIList;
    [SerializeField] List<Transform> AI_SpawnLocations;

    public int SpawnCount = 18;
   
    public bool isBossFightActive = false;
    public UnityEvent OnBossFightComplete;
    public UnityEvent OnBigFightComplete;
    bool m_doCompleteEventOnce = false;

    bool m_isSecondPhase = false;

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

    private void CheckForBigFight()
    {
        if (InstantiatedAI.Count > 0)
        {
            int deathcount = 0;
            for (int i = 0; i < SpawnCount; i++)
            {
                if (InstantiatedAI[i] == null)
                {
                    deathcount++;
                }
            }
        }
    }

    public void StartSpawningAI()
    {
        StartCoroutine(SpawnAICoroutine());
    }

   //Get the sum of all AI's Health 
    private float GetTotalBossHealth()
    {
        float totalhp = 0;
        foreach (AI_Brute brute in AIList)
        { 
            totalhp += brute.GetHealth();

        }
        return totalhp;
    }

    // Update the health Slider UI
    public void UpdateHealthSlider()
    {
        m_BossHealthSlider.value = GetTotalBossHealth();
    }

    // Set Max value for boss's health slider.
    private void SetSliderMaxValue(float _maxValue)
    {
        m_BossHealthSlider.maxValue = _maxValue;
        m_BossHealthSlider.value = m_BossHealthSlider.maxValue;
    }

    // Enable the brute AI script for every brute in the list.
    private void EnableBossAI(bool _b)
    {
        foreach (AI_Brute brute in AIList)
        {
            brute.enabled = _b;

        }
        isBossFightActive = _b;
    }

    // Enable The Canvas and enable the boss AI
    public void EnableBossFight()
    {
        ArenaCanvas.SetActive(true);
        if (m_BossHealthSlider == null)
        {
            m_BossHealthSlider = ArenaCanvas.GetComponentInChildren<Slider>();
        }
        EnableBossAI(true);
        m_Animator.SetTrigger("FadeIn");
    }

    // Disable Boss AI and Canvas
    public void DisableBossFight()
    {
        ArenaCanvas.SetActive(false);
        EnableBossAI(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < AIList.Count; i++)
        {
            AIList[i].UpdateUIEvent += UpdateHealthSlider;
        }
        
        m_Animator = ArenaCanvas.GetComponent<Animator>();

       

    }
    void Update()
    {
        if (isBossFightActive)
        {
            if (AIList.Count > 0)
            {
                foreach (AI_Brute brute in AIList)
                {
                    if (m_BossHealthSlider.maxValue == 1 && brute.GetHealth() != 0)
                    {
                        SetSliderMaxValue(GetTotalBossHealth());
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                foreach (AI_Brute brute in AIList)
                {
                    brute.Damage(Mathf.Infinity, CustomCollider.DamageType.Critical, Vector3.zero);
                }
            }

            if (GetTotalBossHealth() <= 0 && !m_doCompleteEventOnce)
            {

                OnBossFightComplete.Invoke();
                DisableBossFight();

                m_doCompleteEventOnce = true;
                isBossFightActive = false;
            }
            if (GetTotalBossHealth() < AIList[0].Config.maxHealth / 2 && !m_isSecondPhase)
            {
                StartCoroutine(SpawnAICoroutine());
                m_Animator.SetTrigger("Lasers");
                m_isSecondPhase = true;
            }


        }
        else {
            DisableBossFight();
        }
        
        
    }

}
