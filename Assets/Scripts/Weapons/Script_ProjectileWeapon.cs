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
    
    public override void Shoot()
    {
        if (CurMagCount > 0)
        {
            for (int i = 0; i < ShotCount; i++)
            {
                Quaternion ProjectileSpread = FiringPoint.rotation * Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f)) * Quaternion.Euler(Random.Range(0.0f, SpreadAngle / 2), 0, 0);
                GameObject Proj = ObjectPooler.Instance.GetObject(Projectile);

                Proj.transform.position = FiringPoint.position;
                Proj.transform.rotation = ProjectileSpread;

                Script_RCProjectile temp = Proj.GetComponent<Script_RCProjectile>();
                temp.SetDamage(Damage);
                temp.SetlifeTime(ProjectileLifetime);
                temp.SetSpeed(ProjectileForce);


            }

            if (CurMagCount <= 10)
            {
                AS.PlayOneShot(EmptySound,1 - CurMagCount * 0.1f);
            }
            
            AS.PlayOneShot(ShootSound);
            Anim.SetTrigger(ShootHash);

            CurMagCount--;

            if(onAmmoChangeEvent != null)
            {
                onAmmoChangeEvent(CurMagCount);
            }
            ShotTimer = FireRate;   

            Vector3 Rotation = new Vector3(RecoilVec.y,Random.Range(-RecoilVec.x/2,RecoilVec.x/2),0);

            HandEffects.RotateTo += Rotation * 0.5f;
            CamEffects.RotateTo += Rotation;
            CamEffects.ShakeAmplitude += ShotShake;
            CamEffects.FovTo += ShotFov;

            HUD.AmmoCount = CurMagCount;
        }
    }
}
