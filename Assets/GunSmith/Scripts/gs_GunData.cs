using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using gs_Types;

[CreateAssetMenuAttribute(fileName ="New Gun Data",menuName ="Gun Data")]
public class gs_GunData : ScriptableObject
{
    [HideInInspector] public GameObject prefab;
    [HideInInspector] public float Damage;
    [HideInInspector] public float MaxDamage;
    [HideInInspector] public float FireRate;
    [HideInInspector] public float MaxFireRate;
    [HideInInspector] public float RampUpTime;
    [HideInInspector] public float ReloadTime;
    [HideInInspector] public float AmmoCount;
    [HideInInspector] public float SpreadAngle;
    [HideInInspector] public float MinSpreadAngle;
    [HideInInspector] public int ShotCount;
    [HideInInspector] public int MaxShotCount;
    [HideInInspector] public float Recoil;
    [HideInInspector] public float BurstCount;
    [HideInInspector] public float BurstDelay;

    [HideInInspector] public gs_TriggerType TriggerType;
    [HideInInspector] public float ChargeTime;

    [HideInInspector] public gs_ChargeType ChargeType;
    [HideInInspector] public gs_ShotType ShotType;

    [HideInInspector] public GameObject Projectile;
    [HideInInspector] public float ProjectileForce;
    [HideInInspector] public float MaxProjectileForce;
    [HideInInspector] public float ProjectileLifetime;

    [HideInInspector] public float ChargeProjectileForce;
    [HideInInspector] public bool ApplyGravity;

    [HideInInspector] public string Name;

    [HideInInspector] public AudioClip ShootSound;
    [HideInInspector] public AudioClip ReloadSound;
}
