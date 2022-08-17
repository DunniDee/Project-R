using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

using TMPro;

public class Scr_BossArena : MonoBehaviour
{
    [SerializeField] List<AI_Brute> BossAI;

    [SerializeField]Slider BossHealthSlider;

    public GameObject BossCanvas;
    Animator animator;
    public bool isBossFightActive = false;


    public UnityEvent OnBossFightComplete;
    bool doCompleteEventOnce = false;

   
    private float GetTotalBossHealth()
    {
        float totalhp = 0;
        foreach (AI_Brute brute in BossAI)
        { 
            totalhp += brute.GetHealth();

        }
        return totalhp;
    }
    public void UpdateHealthSlider()
    {
        BossHealthSlider.value = GetTotalBossHealth();
    }

    private void SetSliderMaxValue(float _maxValue)
    {
      
        BossHealthSlider.maxValue = _maxValue;
        BossHealthSlider.value = BossHealthSlider.maxValue;
    }

    private void EnableBossAI(bool _b)
    {
        foreach (AI_Brute brute in BossAI)
        {
            brute.enabled = _b;

        }
        isBossFightActive = _b;
    }
    public void EnableBossFight()
    {
        BossCanvas.SetActive(true);
        if (BossHealthSlider == null)
        {
            BossHealthSlider = GetComponentInChildren<Slider>();
        }
        EnableBossAI(true);
        animator.SetTrigger("FadeIn");
    }

    public void DisableBossFight()
    {
        BossCanvas.SetActive(false);
        EnableBossAI(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < BossAI.Count; i++)
        {
            BossAI[i].UpdateUIEvent += UpdateHealthSlider;
        }
        
        animator = BossCanvas.GetComponent<Animator>();


    }
    void Update()
    {
        if(GetTotalBossHealth() <= 0 && !doCompleteEventOnce && isBossFightActive)
        {
            isBossFightActive = false;
            OnBossFightComplete.Invoke();
            doCompleteEventOnce = true;
        }
        foreach (AI_Brute brute in BossAI)
        {
            if (BossHealthSlider.maxValue == 1 && brute.GetHealth() != 0)
            {
                SetSliderMaxValue(GetTotalBossHealth());
            }
        }
        
    }

}
