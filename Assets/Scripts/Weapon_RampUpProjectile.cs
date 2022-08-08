using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_RampUpProjectile : Script_ProjectileWeapon
{
    [SerializeField] float FireRateMultiplier;
    [SerializeField] float RampUpAcceleration;
    public float m_Multiplier;

    private void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        HUD.AmmoCount = CurMagCount;
        FiringPoint.transform.LookAt(Look.LookPoint);

        if (ShotTimer > 0)
        {
            ShotTimer-= Time.deltaTime * m_Multiplier;
        }

        if (Input.GetKey(ShootKey) && CurMagCount > 0 && ShotTimer <= 0 && !IsReloading)
        {
            Shoot();
            m_Multiplier = Mathf.Lerp(m_Multiplier, FireRateMultiplier, Time.deltaTime * RampUpAcceleration);
        }
        else
        {
            m_Multiplier = Mathf.Lerp(m_Multiplier, 1, Time.deltaTime * RampUpAcceleration);
        }

        Reload();

        Animate();
    }
}
