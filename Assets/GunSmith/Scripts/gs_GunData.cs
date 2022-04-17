using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using gs_Types;

[CreateAssetMenuAttribute(fileName ="New Gun Data",menuName ="Gun Data")]
public class gs_GunData : ScriptableObject
{
    public GameObject prefab;
    public float Damage;
    public float MaxDamage;
    public float FireRate;
    public float MaxFireRate;
    public float RampUpTime;
    public float ReloadTime;
    public float AmmoCount;
    public float SpreadAngle;
    public float MinSpreadAngle;
    public int ShotCount;
    public int MaxShotCount;
    public float Recoil;
    public float BurstCount;
    public float BurstDelay;

    public gs_TriggerType TriggerType;
    public float ChargeTime;

    public gs_ChargeType ChargeType;
    public gs_ShotType ShotType;

    public GameObject Projectile;
    public float ProjectileForce;
    public float MaxProjectileForce;
    public float ProjectileLifetime;

    public float ChargeProjectileForce;
    public bool ApplyGravity;

    public string Name;

    public AudioClip ShootSound;
    public AudioClip ReloadSound;
}
