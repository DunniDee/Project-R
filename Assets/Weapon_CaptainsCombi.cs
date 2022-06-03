using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_CaptainsCombi : Script_ProjectileWeapon
{
    [SerializeField] Transform RightFirepoint;
    [SerializeField] Transform CombiFirepoint;
    Transform LeftFirepoint;
    bool IsCombined = false;
    bool IsLeft = false;
    private void Start()
    {
        Initialize();
        LeftFirepoint = FiringPoint;
    }

    // Update is called once per frame
    void Update()
    {
        FiringPoint.transform.LookAt(Look.getAimPoint());


        if (!IsCombined)
        {

            if (Input.GetKey(ShootKey) && CurMagCount > 0 && ShotTimer <= 0 && !IsReloading)
            {
                IsLeft = !IsLeft;
                Anim.SetBool("IsLeft", IsLeft);

                Motor.SetIsSprinting(false);
                Shoot();
            }   

            if (IsLeft)
            {
                FiringPoint = LeftFirepoint;
            }
            else
            {
                FiringPoint = RightFirepoint;
            }

            if (ShotTimer > 0)
            {
                ShotTimer-= Time.deltaTime;
            }
        }
        else
        {
            if (Input.GetKey(ShootKey) && CurMagCount > 0 && ShotTimer <= 0 && !IsReloading)
            {
                FiringPoint = CombiFirepoint;

                Shoot();
                Motor.SetIsSprinting(false);
            }

            if (ShotTimer > 0)
            {
                ShotTimer-= Time.deltaTime;
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            IsCombined = !IsCombined;
            Anim.SetBool("IsCombined", IsCombined);
        }

        Reload();

        Animate();

        UpdateBloom();
    }
}
