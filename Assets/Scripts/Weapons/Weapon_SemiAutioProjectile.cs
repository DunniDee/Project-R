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

    // Update is called once per frame
    void Update()
    {
        HUD.AmmoCount = CurMagCount;
        FiringPoint.transform.LookAt(Look.LookPoint);

        if (ShotTimer > 0)
        { 
            ShotTimer-= Time.deltaTime;
        }

        if (Input.GetKeyDown(ShootKey) && CurMagCount > 0 && ShotTimer <= 0 && !IsReloading)
        {
            Shoot();
        }

        Reload();

        Animate(); 

    }
}
