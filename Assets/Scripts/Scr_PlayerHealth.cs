using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Script Owner: Ashley Rickit

public class Scr_PlayerHealth : MonoBehaviour
{
    [Header("Internal Components")]
    public AudioSource audioSource;
    public AudioClip[] damageNoise;
    public Script_HealthUI m_healthUI;
    public Scr_CameraEffects CamEffects;
    public float maxHealth = 100.0f;
    public float currentHealth = 0.0f;

    public Image BloodyScreenImage;
    public Image HealingScreenImage;

    [Header("Regen Properties")]
    public float RegenRate = 0.1f;
    public float curRegenRate = 0.0f;
    public float MaxRegenRate = 3.0f;


    public delegate void OnTakeDamageDelegate(float Time, float Amplitude);
    public OnTakeDamageDelegate OnTakeDamageEvent;

    #region Member Functions

    /// <summary>
    /// Deal damage to the player and update the players UI. 
    /// </summary>
    /// <param name="_Damage"></param>
    /// <param name="_cameraShakeTime"></param>
    /// <param name="_cameraShakeAmplitude"></param>
    public void TakeDamage(float _Damage,float _cameraShakeTime, float _cameraShakeAmplitude)
    {
        StartCoroutine(FadeInCoroutine(BloodyScreenImage));
        if (OnTakeDamageEvent != null)
        {
            OnTakeDamageEvent(0.2f, 0.5f);
        }
        PlayDamageNoise();

        currentHealth -= _Damage;
        m_healthUI.HealthValueText.text = currentHealth.ToString("F0");
        CamEffects.ShakeTime += _cameraShakeTime;
        CamEffects.ShakeAmplitude += _cameraShakeAmplitude;
    }

    /// <summary>
    /// Add heal amount to the player health and update player UI
    /// </summary>
    /// <param name="_HealAmount"></param>
    public void Heal(float _HealAmount) {
        StartCoroutine(FadeInCoroutine(HealingScreenImage));
        if (currentHealth + _HealAmount > maxHealth)
        {
            currentHealth = maxHealth;
            return;
        }
        currentHealth += _HealAmount;
        m_healthUI.HealthValueText.text = currentHealth.ToString("F0");
    }

    /// <summary>
    /// Play Random Damage Audio Clip
    /// </summary>
    void PlayDamageNoise()
    {
        audioSource.PlayOneShot(damageNoise[Random.Range(0,damageNoise.Length)]);
    }

    /// <summary>
    /// Reset Player Health to max Health
    /// </summary>
    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Fade hurtImage or Heal image in from 0 opacity.
    /// </summary>
    /// <param name="_imageToFade"></param>
    /// <returns></returns>
    public IEnumerator FadeInCoroutine(Image _imageToFade)
    {
        for (float f = 0.05f; f < 0.3; f += 0.05f)
        {
            Color c = _imageToFade.color;
            c.a = f;
            _imageToFade.color = c;
            yield return new WaitForSeconds(0.05f);
        }
        StartCoroutine(FadeOutCoroutine(_imageToFade));
    }

    /// <summary>
    /// Fade hurtimage or heal image out from 0.3 opacity
    /// </summary>
    /// <param name="_imageToFade"></param>
    /// <returns></returns>
    public IEnumerator FadeOutCoroutine(Image _imageToFade)
    {
        for (float f = 0.05f; f >= 0; f -= 0.02f)
        {
            Color c = _imageToFade.color;
            c.a = f;
            _imageToFade.color = c;
            yield return new WaitForSeconds(0.02f);
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        m_healthUI = GetComponentInChildren<Script_HealthUI>();
        CamEffects = GetComponentInChildren<Scr_CameraEffects>();
        
        //Initalise Color Values
        Color c = BloodyScreenImage.color;
        c.a = 0f;
        BloodyScreenImage.color = c;

        Color ca = HealingScreenImage.color;
        ca.a = 0f;
        HealingScreenImage.color = ca;

    
        currentHealth = maxHealth;
    }

   
    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Script_SceneManager.Instance.LoadScene("MainMenu");
        }
        
    }

   
}
