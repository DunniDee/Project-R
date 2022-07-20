using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_BurstProjectile : Script_ProjectileWeapon
{
    [SerializeField] int BurstCount;
    [SerializeField] float BurstSpeed;
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
            for (int i = 0; i < BurstCount; i++)
            {
                Invoke("Shoot", BurstSpeed * i);
            }
        }

        Reload();

        Animate();
    }
}
