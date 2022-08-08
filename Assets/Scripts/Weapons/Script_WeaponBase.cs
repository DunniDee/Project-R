using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Script_WeaponBase : MonoBehaviour
{
    [Header("Gun Variables")]
    [SerializeField] protected string GunName;
    [SerializeField] protected KeyCode ShootKey = KeyCode.Mouse0;
    [SerializeField] protected KeyCode ReloadKey = KeyCode.R;
    [Space]

    [Header("Shot Variables")]
    [SerializeField] protected float Damage;
    [SerializeField] protected float FireRate;
    public float ShotTimer;
    [SerializeField] protected int ShotCount;
    [SerializeField] protected float SpreadAngle;
    [SerializeField] protected Vector2 RecoilVec;
    [Space]

   

    [Header("Reload Variables")]
    [SerializeField] protected int MaxReserveCount;
    [SerializeField] protected int CurReserveCount;
    [SerializeField] protected int MagCount;
    public int CurMagCount;
    [SerializeField] protected float ReloadTime;
    protected float CurReloadTime;
    public bool IsReloading = false;

    [Space]

    [Header("Recoil Variables")]
    [SerializeField] protected float ShotShake;
    [SerializeField] protected float ShotFov;

    [Space]

    [Header("Audio Clips")]
    [SerializeField] protected AudioClip ShootSound; // angle to recoil by
    [SerializeField] protected AudioClip EmptySound; // angle to recoil by
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

    //Hidden Variables
    protected Scr_PlayerMotor Motor;
    protected Scr_PlayerLook Look;
    protected AudioSource AS;

    //Delegate for Ammo UI    
    public delegate void OnAmmoChangeDelegate(int _ammo);
    public OnAmmoChangeDelegate onAmmoChangeEvent;

    [SerializeField] protected Scr_DiegeticHUD HUD;
    [SerializeField] protected Scr_HandAnimator HandEffects;
    [SerializeField] protected Scr_CameraEffects CamEffects;

    //Need reference to the weapon swap script to know what index the weapon is.
    [SerializeField] protected script_WeaponSwap WeaponSwap;
    protected private void OnEnable() 
    {
       HUD.MagSize = MagCount;
       HUD.AmmoCount = CurMagCount;
       HUD.SetGunName(GunName);

    }

    #region Getters And Setters

    public string GetGunName()
    {
        return GunName;
    }
    public int GetMagCount()
    {
        return MagCount;
    }

    public float GetDamage()
    {
        return Damage;
    }
    public void SetDamage(float _Damage)
    {
        Damage = _Damage;
    }

    public float GetFireRate()
    {
        return FireRate;
    }
    public void SetFireRate(float _FireRate)
    {
        FireRate = _FireRate;
    }

    #endregion

    protected virtual IEnumerator IE_Reload()
    {
        IsReloading = true;
        yield return new WaitForSeconds(ReloadTime);
        IsReloading = false;
        
        CurReserveCount-= MagCount - CurMagCount;

        if (CurReserveCount < 0)
        {
            int LeftOver = Mathf.Abs(CurReserveCount);
            CurReserveCount += LeftOver;
            CurMagCount = MagCount - LeftOver;
        }
        else
        {
            CurMagCount = MagCount;
        }
        
        if(onAmmoChangeEvent != null)
        {
            onAmmoChangeEvent(CurMagCount);
        }

        HUD.AmmoReserve = CurReserveCount;
    }

    public virtual void Shoot(){}
    public virtual void ShootNoAnim(){}
    protected virtual void Reload()
    {
        if ((CurMagCount < 1 && ShotTimer < 1 && !IsReloading && CurReserveCount > 0) ||
            (CurMagCount < MagCount && ShotTimer < 1 && !IsReloading && Input.GetKey(ReloadKey) && CurReserveCount > 0))
        {
            StartCoroutine(IE_Reload());
            AS.PlayOneShot(ReloadSound);
            Anim.SetTrigger(ReloadHash);
        }
    }

    protected virtual void Initialize() // make sure to call initialise in the start of other sublcasses
    {
        Motor = gameObject.GetComponentInParent<Scr_PlayerMotor>();
        Look = gameObject.GetComponentInParent<Scr_PlayerLook>();
        AS = gameObject.GetComponent<AudioSource>();
        WeaponSwap = gameObject.GetComponentInChildren<script_WeaponSwap>();

        CurMagCount = MagCount;

        ShootHash = Animator.StringToHash("shoot");
        ReloadHash = Animator.StringToHash("reload");
        zVelHash = Animator.StringToHash("zVelocity");
        xVelHash = Animator.StringToHash("xVelocity");
    }

    protected void Animate() // make sure to call in update
    {
        Anim.SetFloat(zVelHash, m_zVelocity);
        Anim.SetFloat(xVelHash, m_xVelocity);
    }
}
