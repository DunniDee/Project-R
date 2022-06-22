using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlayerHealth : MonoBehaviour
{
    [Header("Internal Components")]

    public Script_HealthUI m_healthUI;
    public float maxHealth = 100.0f;
    public float currentHealth = 0.0f;

    [Header("Regen Properties")]
    public float RegenRate = 0.1f;
    public float curRegenRate = 0.0f;
    public float MaxRegenRate = 3.0f;

    private float timer = 0.0f;
    private float waitTime = 5.0f;

    public delegate void OnTakeDamageDelegate(float Time, float Amplitude);
    public OnTakeDamageDelegate OnTakeDamageEvent;

    public void TakeDamage(float _Damage)
    {
        if(OnTakeDamageEvent != null)
        {
            OnTakeDamageEvent(0.2f,0.5f);
            timer = waitTime;
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
        if (currentHealth <= 0)
        {
            Script_SceneManager.Instance.LoadScene("HubWorld");
        }

        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }
        else if (timer <= 0.0f && currentHealth < maxHealth)
        {
            currentHealth += curRegenRate;
            currentHealth += RegenRate;
        }
    }
}
