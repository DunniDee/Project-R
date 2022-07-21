using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_FullAutoProjectile : Script_ProjectileWeapon
{
<<<<<<< Updated upstream
    [SerializeField] Scr_DiegeticHUD HUD;
    [SerializeField] Scr_HandAnimator HandEffects;
    [SerializeField] Scr_CameraEffects CamEffects;
=======
    private void OnEnable() 
    {
        HUD.AmmoCount = CurMagCount;
        HUD.MagSize = MagCount;
        HUD.SetGunName(GunName);
    }

>>>>>>> Stashed changes
    private void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        FiringPoint.transform.LookAt(Look.LookPoint);

        if (ShotTimer > 0)
        {
            ShotTimer-= Time.deltaTime;
        }

        if (Input.GetKey(ShootKey) && CurMagCount > 0 && ShotTimer <= 0 && !IsReloading)
        {
            Shoot();
            HandEffects.RotateTo += new Vector3(RecoilVec.y,Random.Range(-RecoilVec.x/2,RecoilVec.x/2),0);
            CamEffects.RotateTo += new Vector3(RecoilVec.y,Random.Range(-RecoilVec.x/2,RecoilVec.x/2),0);
            CamEffects.ShakeAmplitude += FireRate;
        }


        HUD.AmmoReserve = CurReserveCount;
        HUD.AmmoCount = CurMagCount;
        HUD.MagSize = MagCount;

        HUD.SetGunName(GunName);

        Reload();

        Animate();
    }
}
