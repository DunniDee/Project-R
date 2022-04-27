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
        if (CurMagCount > 0)
        {
            for (int i = 0; i < ShotCount; i++)
            {
                Quaternion ProjectileSpread = FiringPoint.rotation * Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f)) * Quaternion.Euler(Random.Range(0.0f, CurrentSpreadAngle / 2), 0, 0);
                GameObject Proj = ObjectPooler.Instance.GetObject(Projectile);

                Proj.transform.position = FiringPoint.position;
                Proj.transform.rotation = ProjectileSpread;

                Rigidbody RB = Proj.GetComponent<Rigidbody>();
                if (RB != null)
                {   
                    RB.useGravity = ApplyGravity;
                    RB.AddForce(Proj.transform.forward * ProjectileForce, ForceMode.VelocityChange);
                }
                else
                {
                    Script_RCProjectile temp = Proj.GetComponent<Script_RCProjectile>();
                    temp.SetDamage(Damage);
                    temp.SetlifeTime(ProjectileLifetime);
                    temp.SetSpeed(ProjectileForce);
                }
            }

            SetBloom();
            SetRecoil();
            
            AS.PlayOneShot(ShootSound);
            Anim.SetTrigger(ShootHash);

            CurMagCount--;
            ShotTimer = FireRate;   
        }
    }
}
