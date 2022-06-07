using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_CaptainsCombi : Script_ProjectileWeapon
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

    private void Start()
    {
        Initialize();
        LeftFirepoint = FiringPoint;
        ShootSound = UnComShotSound;

    }

    // Update is called once per frame
    void Update()
    {
        FiringPoint.transform.LookAt(Look.getAimPoint());


        if (!IsCombined)
        {
            if (Input.GetKey(ShootKey) && CurMagCount > 0 && ShotTimer <= 0 && !IsReloading)
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
            if (Input.GetKey(ShootKey) && CurMagCount > 0 && ShotTimer <= 0 && !IsReloading)
            {
                FiringPoint = CombiFirepoint;

                Shoot();
                Motor.SetIsSprinting(false);
            }

            if (ShotTimer > 0)
            {
                ShotTimer-= Time.deltaTime * CombiFireateModifier;
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            IsCombined = !IsCombined;
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


}
