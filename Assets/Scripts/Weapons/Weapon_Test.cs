using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_FullAutoProjectileWeapon : Script_ProjectileWeapon
{
    private void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        FiringPoint.transform.LookAt(Look.getAimPoint());

        if (ShotTimer > 0)
        {
            ShotTimer-= Time.deltaTime;
        }

        if (Input.GetKey(ShootKey) && CurMagCount > 0 && ShotTimer <= 0 && !IsReloading)
        {
            Shoot();
            Motor.SetIsSprinting(false);
        }

        Reload();

        Animate();
    }
}
