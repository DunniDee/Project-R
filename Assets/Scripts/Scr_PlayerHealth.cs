using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlayerHealth : MonoBehaviour
{
    [Header("Internal Components")]
    public AudioSource audioSource;
    public AudioClip[] damageNoise;
    public Script_HealthUI m_healthUI;
    public Scr_CameraEffects CamEffects;
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

    public void TakeDamage(float _Damage,float _cameraShakeTime, float _cameraShakeAmplitude)
    {
        if (OnTakeDamageEvent != null)
        {
            OnTakeDamageEvent(0.2f, 0.5f);
            timer = waitTime;
        }
        PlayDamageNoise();

        currentHealth -= _Damage;
        m_healthUI.HealthValueText.text = currentHealth.ToString("F0");
        CamEffects.ShakeTime += _cameraShakeTime;
        CamEffects.ShakeAmplitude += _cameraShakeAmplitude;
    }
    public void Heal(float _HealthAmount) {
        currentHealth += _HealthAmount;
        m_healthUI.HealthValueText.text = currentHealth.ToString("F0");
    }
    void PlayDamageNoise()
    {
        audioSource.PlayOneShot(damageNoise[Random.Range(0,damageNoise.Length)]);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        m_healthUI = GetComponentInChildren<Script_HealthUI>();
        CamEffects = GetComponentInChildren<Scr_CameraEffects>();
        currentHealth = maxHealth;
        
    }

   
    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Script_SceneManager.Instance.LoadScene("MainMenu");
        }

        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }
 
    }

   
}
