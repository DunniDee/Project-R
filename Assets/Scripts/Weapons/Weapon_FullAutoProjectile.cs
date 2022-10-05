using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_FullAutoProjectile : Script_ProjectileWeapon
{
    private void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        HUD.AmmoCount = CurMagCount;
        HUD.AmmoReserve = CurReserveCount;
        FiringPoint.transform.LookAt(Look.LookPoint);

        
        Reload();
        ReloadUpdate();
        
        if (ShotTimer > 0)
        {
            ShotTimer-= Time.deltaTime;
        }

        if (Input.GetKey(ShootKey) && CurMagCount > 0 && ShotTimer <= 0 && (!IsReloading || SingleReload && MagCount > 0))
        {
            IsReloading = false;
            Shoot();
        }

        Animate();
    }
}
