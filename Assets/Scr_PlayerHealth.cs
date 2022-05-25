using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlayerHealth : MonoBehaviour
{
    [Header("Internal Components")]

    public Script_HealthUI m_healthUI;
    public float maxHealth = 100.0f;
    public float currentHealth = 0.0f;

    public delegate void OnTakeDamageDelegate(float Time, float Amplitude);
    public OnTakeDamageDelegate OnTakeDamageEvent;

    public void TakeDamage(float _Damage)
    {
        if(OnTakeDamageEvent != null)
        {
            OnTakeDamageEvent(0.2f,0.5f);
        }
        currentHealth -= _Damage;
        m_healthUI.m_healthSlider.value = currentHealth;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        m_healthUI = GetComponent<Script_HealthUI>();
        m_healthUI.m_healthSlider.maxValue = maxHealth;
        m_healthUI.m_healthSlider.value = currentHealth;

        m_healthUI.m_followSlider.maxValue = maxHealth;
        m_healthUI.m_followSlider.value = currentHealth;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
