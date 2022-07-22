using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltFire_FanHammer : Scr_AltFireBase
{
    [SerializeField] float ShotInterval;
    private void OnEnable()
    {
        HUD.SetAltFireName(AltFireName);
    }

    protected override void OnAbilityStart()
    {
        base.OnAbilityStart();
        if (Weapon.CurMagCount > 0 && Weapon.ShotTimer <= 0 && !Weapon.IsReloading)
        {
            for (int i = 0; i < Weapon.GetMagCount(); i++)
            {
                Invoke("Shoot", i * ShotInterval);
            }
        }
    }

    void Shoot()
    {
        Weapon.Shoot();
    }
}
