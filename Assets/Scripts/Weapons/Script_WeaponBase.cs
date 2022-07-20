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
     protected float ShotTimer;
    [SerializeField] protected int ShotCount;
    [SerializeField] protected float SpreadAngle;
    [SerializeField] protected Vector2 RecoilVec;
    [Space]

    [Header("Reload Variables")]
    [SerializeField] protected int MaxReserveCount;
    [SerializeField] protected int CurReserveCount;
    [SerializeField] protected int MagCount;
    protected int CurMagCount;
    [SerializeField] protected float ReloadTime;
    protected float CurReloadTime;
    protected bool IsReloading = false;

    [Space]

    [Header("Recoil Variables")]

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

    [SerializeField] protected ParticleSystem reloadsmoke;
    [SerializeField] protected ParticleSystem reloadsmoke2;

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
            reloadsmoke.Play();
            reloadsmoke2.Play();
        }
    }

    protected virtual void Initialize() // make sure to call initialise in the start of other sublcasses
    {
        Motor = gameObject.GetComponentInParent<Scr_PlayerMotor>();
        Look = gameObject.GetComponentInParent<Scr_PlayerLook>();
        AS = gameObject.GetComponent<AudioSource>();

        CurMagCount = MagCount;

        ShootHash = Animator.StringToHash("Shoot");
        ReloadHash = Animator.StringToHash("Reload");
        zVelHash = Animator.StringToHash("zVelocity");
        xVelHash = Animator.StringToHash("xVelocity");
    }

    protected void Animate() // make sure to call in update
    {
        Anim.SetFloat(zVelHash, m_zVelocity);
        Anim.SetFloat(xVelHash, m_xVelocity);
    }
}
