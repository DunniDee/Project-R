using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scr_BossArena : MonoBehaviour
{
    [SerializeField] AI_Brute BossAI;

    [SerializeField]Slider BossHealthSlider;

    public GameObject BossCanvas;
    public void UpdateHealthSlider()
    {
        BossHealthSlider.value = BossAI.GetHealth();
    }

    private void SetSliderMaxValue(float _maxValue)
    {
      
        BossHealthSlider.maxValue = _maxValue;
        BossHealthSlider.value = BossHealthSlider.maxValue;
    }

    public void EnableBossFight()
    {
        BossCanvas.SetActive(true);
        if (BossHealthSlider == null)
        {
            BossHealthSlider = GetComponentInChildren<Slider>();
        }
        SetSliderMaxValue(BossAI.GetHealth());
        BossAI.enabled = true;
    }

    public void DisableBossFight()
    {
        BossCanvas.SetActive(false);
        BossAI.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (BossAI.enabled) { BossAI.enabled = false; } 
  

       
    }

}
