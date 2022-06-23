using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_CaptainsCombi : Script_ProjectileWeapon, IUpgradable
{
    [SerializeField] Transform RightFirepoint;
    [SerializeField] Transform CombiFirepoint;
    Transform LeftFirepoint;
    bool IsCombined = false;
    bool IsLeft = false;

    [SerializeField] float CombiFireateModifier;
    [SerializeField] float CombiDamage;
    [SerializeField] float UnComDamage;
    [SerializeField] float CombiADS;
    [SerializeField] float UnComADS;

    [SerializeField] float CombiMinSpread;
    [SerializeField] float CombiMaxSpread;
    [SerializeField] float CombiADSMinSpread;
    [SerializeField] float CombiADSMaxSpread;

    [SerializeField] float UnComMinSpread;
    [SerializeField] float UnComMaxSpread;
    [SerializeField] float UnComADSMinSpread;
    [SerializeField] float UnComADSMaxSpread;

    [SerializeField] AudioClip CombiShotSound;
    [SerializeField] AudioClip UnComShotSound;

    [SerializeField] float CombiTime;
    float CombiTimer;

    public void InitialiseStats()
    {
        Script_PlayerStatManager.Instance.DefaultPrimaryDamage = UnComDamage;
        UnComDamage =Script_PlayerStatManager.Instance.ModifiedPrimaryDamage;

        Script_PlayerStatManager.Instance.DefaultSecondaryDamage = CombiDamage;
        CombiDamage =Script_PlayerStatManager.Instance.ModifiedSecondaryDamage;

        Script_PlayerStatManager.Instance.DefaultPrimaryFireRate = FireRate;
        FireRate =Script_PlayerStatManager.Instance.ModifiedPrimaryFireRate;

        Script_PlayerStatManager.Instance.DefaultSecondaryFireRate = CombiFireateModifier;
        CombiFireateModifier =Script_PlayerStatManager.Instance.ModifiedSecondaryFireRate;

        Script_PlayerStatManager.Instance.DefaultPrimaryMagCount = MagCount;
        MagCount =Script_PlayerStatManager.Instance.ModifiedPrimaryMagCount;
    }

    private void Start()
    {
        InitialiseStats();
        Initialize();
        LeftFirepoint = FiringPoint;
        ShootSound = UnComShotSound;

    }

    // Update is called once per frame
    void Update()
    {
        FiringPoint.transform.LookAt(Look.getAimPoint());

        if (CombiTimer >= 0)
        {
            CombiTimer-= Time.deltaTime;
        }


        if (!IsCombined)
        {
            if (Input.GetKey(ShootKey) && CurMagCount > 0 && ShotTimer <= 0 && !IsReloading && CombiTimer < 0)
            {
                IsLeft = !IsLeft;
                Anim.SetBool("IsLeft", IsLeft);

                Motor.SetIsSprinting(false);
                Shoot();
            }   

            if (IsLeft)
            {
                FiringPoint = LeftFirepoint;
            }
            else
            {
                FiringPoint = RightFirepoint;
            }

            if (ShotTimer > 0)
            {
                ShotTimer-= Time.deltaTime;
            }
        }
        else
        {
            if (ADS)
            {
                if (Input.GetKey(ShootKey) && CurMagCount > 0 && ShotTimer <= 0 && !IsReloading && CombiTimer < 0)
                {
                    FiringPoint = CombiFirepoint;
                    for (int i = 0; i < 3; i++)
                    {
                        Invoke("Shoot", 0.085f * i);
                    }

                    Motor.SetIsSprinting(false);
                }
            }
            else
            {
                if (Input.GetKey(ShootKey) && CurMagCount > 0 && ShotTimer <= 0 && !IsReloading && CombiTimer < 0)
                {
                    FiringPoint = CombiFirepoint;

                    Shoot();
                    Motor.SetIsSprinting(false);
                }
            }

            if (ShotTimer > 0)
            {
                ShotTimer-= Time.deltaTime * CombiFireateModifier;
            }
        }


        if (Input.GetKeyDown(KeyCode.Tab))
        {
            IsCombined = !IsCombined;
            
            CombiTimer = CombiTime;
            Anim.SetBool("IsCombined", IsCombined);

            if (IsCombined)
            {
                SetCombiStats();
            }
            else
            {
                SetUnComStats();
            }
        }

        Reload();

        Animate();

        UpdateBloom();
    }

    void SetCombiStats()
    {
        Damage = CombiDamage;
        ADSFovZoom = CombiADS;

        defMinSpreadAngle = CombiMinSpread;
        defMaxSpreadAngle = CombiMaxSpread;
        ADSMinSpreadAngle = CombiADSMinSpread;
        ADSMaxSpreadAngle = CombiADSMaxSpread;

        ShootSound = CombiShotSound;
    }
    void SetUnComStats()
    {
        Damage = UnComDamage;
        ADSFovZoom = UnComADS;

        defMinSpreadAngle = UnComMinSpread;
        defMaxSpreadAngle = UnComMaxSpread;
        ADSMinSpreadAngle = UnComADSMinSpread;
        ADSMaxSpreadAngle = UnComADSMaxSpread;

        ShootSound = UnComShotSound;
    }

    public int getCurMagCount()
    {
        return CurMagCount;
    }
}
