using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_SemiAutioProjectile : Script_ProjectileWeapon
{
    // Start is called before the first frame update
    private void Start()
    {
        Initialize();
    }

    /// <summary>
    /// bundles all the functionality from the base classs with the difference based on the input type
    /// </summary>
    void Update()
    {
        HUD.AmmoCount = CurMagCount;
        HUD.AmmoReserve = CurReserveCount;
        FiringPoint.transform.LookAt(Look.LookPoint);

        if (ShotTimer > 0)
        { 
            ShotTimer-= Time.deltaTime;
        }

        if (Input.GetKeyDown(ShootKey) && CurMagCount > 0 && ShotTimer <= 0 && (!IsReloading || SingleReload && MagCount > 0))
        {
            Shoot();
        }

        Reload();
        ReloadUpdate();

        Animate(); 

    }
}
