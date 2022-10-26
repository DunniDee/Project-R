using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Script Owner: Ashley Rickit

public class Scr_PlayerHealth : MonoBehaviour
{
    [Header("Internal Components")]
    public AudioSource audioSource;
    public AudioClip[] damageNoise;
    public Script_HealthUI m_healthUI;
    public Scr_CameraEffects CamEffects;

    [Header("Death Components")]
    public GameObject DeathScreen;
    public CanvasGroup DeathCanvas;

    public Scr_PlayerMotor playerMotor;
    public Scr_PlayerLook playerLook;
    public script_WeaponSwap weaponSwap;

    //Disable ondeath so that it doesnt block input on other canvas's
    public GameObject ScreenSpaceUI;

    [Header("Health & Damage Properties")]
    public float maxHealth = 100.0f;
    public float currentHealth = 0.0f;

    public Image BloodyScreenImage;
    public Image HealingScreenImage;

    [Header("Regen Properties")]
    public float RegenRate = 1;
    public float curRegenRate = 0.0f;
    public float MaxRegenRate = 3.0f;
    private float CurrentRegenTimer = 0.0f;
    private float MaxRegenTimer = 5.0f;


    public delegate void OnTakeDamageDelegate(float Time, float Amplitude);
    public OnTakeDamageDelegate OnTakeDamageEvent;

    public delegate void OnDeathDelegate();
    public OnDeathDelegate OnDeathEvent;

    bool isDead = false;

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
        CurrentRegenTimer = MaxRegenTimer;
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

    /// <summary>
    /// Lerps player Death canvas object to _targetValue over _duration.
    /// </summary>
    /// <param name="targetValue"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    IEnumerator FadeDeathScreen(float targetValue, float duration)
    {
        float startValue = DeathCanvas.alpha;
        float time = 0;
        while (time < duration)
        {
            DeathCanvas.alpha = Mathf.Lerp(startValue, targetValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        DeathCanvas.alpha = targetValue;
    }

    /// <summary>
    /// calls FadeDeathCanvas Coroutine and enables cursor.
    /// </summary>
    /// <param name="_targetValue"></param>
    /// <param name="_duration"></param>
    public void ShowDeathCanvas(float _targetValue, float _duration)
    {
        DeathScreen.SetActive(true);

        StartCoroutine(FadeDeathScreen(_targetValue, _duration));
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    /// <summary>
    /// Enable or disable Player components so that they cannot move or interact.
    /// only used on player death or respawning.
    /// </summary>
    /// <param name="_b"></param>
    public void EnablePlayerComponents(bool _b)
    {
        playerMotor.enabled = _b;
        playerLook.enabled = _b;
        weaponSwap.EquippedWeapons[weaponSwap.EquippedIndex].SetActive(_b);
        ScreenSpaceUI.SetActive(_b);
    }

    public void RespawnPlayer()
    {
        EnablePlayerComponents(true);
        ResetHealth();
        StartCoroutine(FadeDeathScreen(0, 0.1f));
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isDead = false;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerMotor = GetComponent<Scr_PlayerMotor>();
        playerLook = GetComponent<Scr_PlayerLook>();

        weaponSwap = GetComponentInChildren<script_WeaponSwap>();
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
        if (CurrentRegenTimer > 0.0f)
        {
            CurrentRegenTimer -= Time.deltaTime;
        }
        else if (CurrentRegenTimer <= 0.0f && currentHealth < maxHealth)
        {
            currentHealth += RegenRate;
        }

        if (currentHealth <= 0 && isDead == false)
        {
            ShowDeathCanvas(1.0f,0.25f);
            EnablePlayerComponents(false);
            isDead = true;
          
           
        }
        
    }

   
}
