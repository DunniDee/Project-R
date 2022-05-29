using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Script_WeaponBase : MonoBehaviour
{
    [Header("Gun Variables")]
    [SerializeField] protected string GunName;
    [SerializeField] protected Sprite Icon;
    [SerializeField] protected KeyCode ShootKey = KeyCode.Mouse0;
    [SerializeField] protected KeyCode ReloadKey = KeyCode.R;

    [Space]

    [Header("Shot Variables")]
    [SerializeField] protected float Damage;
    [SerializeField] protected float FireRate;
    [SerializeField] protected float MinSpreadAngle;
    [SerializeField] protected float MaxSpreadAngle;
    protected float defMinSpreadAngle;
    protected float defMaxSpreadAngle;
    [SerializeField] protected float SpreadIncrement;
    [SerializeField] protected float SpreadSlerp;
    [SerializeField] protected float CurrentSpreadAngle;
     protected float ShotTimer;
    [SerializeField] protected int ShotCount;

    [Header("Shot Variables")]
    [SerializeField] protected float ADSFovZoom;
    [SerializeField] protected float ADSMinSpreadAngle;
    [SerializeField] protected float ADSMaxSpreadAngle;
    protected bool ADS;

    [Space]

    [Header("Reload Variables")]
    [SerializeField] protected int MagCount;
    protected int CurMagCount;
    [SerializeField] protected float ReloadTime;
    protected float CurReloadTime;
    protected bool IsReloading = false;

    [Space]

    [Header("Recoil Variables")]
    [SerializeField] protected float Recoil; // angle to recoil by
    [SerializeField] protected float RecoilTime; // time it takes to return to forward looking
    [SerializeField] protected float SlerpSpeed; // speed it slerps to the specified recoil angle
    [SerializeField] protected float ShakeTime; // time the screen shakes for
    [SerializeField] protected float ShakeAmplitude; // amplitude of the screen shake

    [SerializeField] protected float ADSShakeMultiplier; // Multipliy the shake when ads
    [SerializeField] protected float ADSRecoilMultiplier; // Multiply the recoil when ads

    [Space]

    [Header("Audio Clips")]
    [SerializeField] protected AudioClip ShootSound; // angle to recoil by
    [SerializeField] protected AudioClip ReloadSound; // angle to recoil by

    [Space]

    [Header("Shoot Position")]
    [SerializeField] protected Transform FiringPoint; // where the bullets come out of

    [Space]

    [Header("Animation Variables")]
    [SerializeField] protected Animator Anim;
    protected int ShootHash;
    protected int ReloadHash;
    protected float m_zVelocity;
    protected float m_xVelocity;
    protected int zVelHash;
    protected int xVelHash;
    protected int ADSHash;

    //Hidden Variables
    protected Script_AdvancedMotor Motor;
    protected Script_PlayerLook Look;
    protected AudioSource AS;

    //Delegate for Ammo UI    
    public delegate void OnAmmoChangeDelegate(int _ammo);
    public OnAmmoChangeDelegate onAmmoChangeEvent;

    public int GetMagCount()
    {
        return MagCount;
    }
    
    protected virtual IEnumerator IE_Reload()
    {
        IsReloading = true;
        yield return new WaitForSeconds(ReloadTime);
        IsReloading = false;
        CurMagCount = MagCount;
        if(onAmmoChangeEvent != null)
        {
            onAmmoChangeEvent(CurMagCount);
        }
    }

    protected virtual void Shoot(){}
    protected virtual void Reload()
    {
        if ((CurMagCount < 1 && ShotTimer < 1 && !IsReloading) ||
            (CurMagCount < MagCount && ShotTimer < 1 && !IsReloading && Input.GetKey(ReloadKey)))
        {
            StartCoroutine(IE_Reload());
            AS.PlayOneShot(ReloadSound);
            Anim.SetTrigger(ReloadHash);
           
        }
    }

    protected virtual void Initialize() // make sure to call initialise in the start of other sublcasses
    {
        Motor = gameObject.GetComponent<Script_AdvancedMotor>();
        Look = gameObject.GetComponent<Script_PlayerLook>();
        AS = gameObject.GetComponent<AudioSource>();

        CurMagCount = MagCount;

        ShootHash = Animator.StringToHash("Shoot");
        ReloadHash = Animator.StringToHash("Reload");
        zVelHash = Animator.StringToHash("zVelocity");
        xVelHash = Animator.StringToHash("xVelocity");
        ADSHash = Animator.StringToHash("ADS");
        
        defMaxSpreadAngle = MaxSpreadAngle;
        defMinSpreadAngle = MinSpreadAngle;
        Look.SetCrosshairSize(CurrentSpreadAngle);
    }

    protected void Animate() // make sure to call in update
    {
        if (Motor.GetIsSprinting())
        {
            m_zVelocity = Mathf.Lerp(m_zVelocity,Motor.getRawDirection().y * 2, Time.deltaTime * 5);
        }
        else
        {
            m_zVelocity = Mathf.Lerp(m_zVelocity,Motor.getRawDirection().y, Time.deltaTime * 5);
        }
        m_xVelocity = Mathf.Lerp(m_xVelocity,Motor.getRawDirection().x, Time.deltaTime * 5);

        Anim.SetFloat(zVelHash, m_zVelocity);
        Anim.SetFloat(xVelHash, m_xVelocity);

        if (Input.GetKey(KeyCode.Mouse1) && !IsReloading) // ADS
        {
            ADS = true;
            Look.SetLerpFov(-ADSFovZoom);
            Motor.SetIsSprinting(false);
        }else
        {
            ADS = false;
            Look.SetLerpFov(0);
        }
        Anim.SetBool(ADSHash,ADS);
    }

    protected void SetRecoil()
    {
        if (ADS)
        {
            Look.SetShake(ShakeTime, ShakeAmplitude * ADSShakeMultiplier);
            Look.SetRecoil(SlerpSpeed,RecoilTime,Quaternion.Euler(-Recoil,Random.Range(-Recoil/2,Recoil/2) * ADSRecoilMultiplier,0));
        }
        else
        {
            Look.SetShake(ShakeTime, ShakeAmplitude);
            Look.SetRecoil(SlerpSpeed,RecoilTime,Quaternion.Euler(-Recoil,Random.Range(-Recoil/2,Recoil/2),0));
        }
    }

    protected void UpdateBloom()
    {
        if (ADS)
        {
            MaxSpreadAngle = ADSMaxSpreadAngle;
            MinSpreadAngle = ADSMinSpreadAngle;
        }
        else
        {
            MaxSpreadAngle = defMaxSpreadAngle;
            MinSpreadAngle = defMinSpreadAngle;
        }

        CurrentSpreadAngle -= Time.deltaTime * SpreadSlerp;
        CurrentSpreadAngle = Mathf.Clamp(CurrentSpreadAngle,MinSpreadAngle,MaxSpreadAngle);
        Look.SetCrosshairSize(CurrentSpreadAngle);
    }

    protected void SetBloom()
    {
        CurrentSpreadAngle += SpreadIncrement;
        CurrentSpreadAngle = Mathf.Clamp(CurrentSpreadAngle,MinSpreadAngle,MaxSpreadAngle);
    }
}
