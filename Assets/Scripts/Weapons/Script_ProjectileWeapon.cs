using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ProjectileWeapon : Script_WeaponBase
{
    [Space]

    [Header("Projectile")]
    [SerializeField] protected GameObject Projectile;
    [SerializeField] protected float ProjectileLifetime;
    [SerializeField] protected float ProjectileForce;
    [SerializeField] protected bool ApplyGravity;


    protected override void Shoot()
    {
        for (int i = 0; i < ShotCount; i++)
        {
            Quaternion ProjectileSpread = FiringPoint.rotation * Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f)) * Quaternion.Euler(Random.Range(0.0f, SpreadAngle / 2), 0, 0);

            GameObject Proj = Instantiate(Projectile, FiringPoint.position, ProjectileSpread);
            Rigidbody RB = Proj.GetComponent<Rigidbody>();
            RB.useGravity = ApplyGravity;
            RB.AddForce(Proj.transform.forward * ProjectileForce, ForceMode.VelocityChange);
            GameObject.Destroy(Proj, ProjectileLifetime);
        }

        SetRecoil();
        AS.PlayOneShot(ShootSound);
        Anim.SetTrigger(ShootHash);

        CurMagCount--;
        ShotTimer = FireRate;
    }
}
