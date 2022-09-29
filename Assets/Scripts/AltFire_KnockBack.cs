using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltFire_KnockBack : Scr_AltFireBase
{
    // Start is called before the first frame update
    [SerializeField] Scr_PlayerMotor Motor;

    [SerializeField] Transform Look;
    [SerializeField] float KnockBackForce;

    protected override void OnAbilityStart()
    {
        base.OnAbilityStart();
        for (int i = 0; i < 4; i++)
        {
            Invoke("Shoot", i * 0.1f);
        }

        //Shoot();
    }

    void Shoot()
    {
        if (Weapon.CurMagCount > 0 && Weapon.ShotTimer <= 0 && !Weapon.IsReloading)
        {
            Weapon.Shoot();
            Vector3 KnockBackDir = -Look.forward;
            Motor.m_MomentumDirection += new Vector3(KnockBackDir.x,0,KnockBackDir.z) * KnockBackForce;
            Motor.m_VerticalVelocity = new Vector3(0,KnockBackDir.y,0) * KnockBackForce;
        }
    }
}
