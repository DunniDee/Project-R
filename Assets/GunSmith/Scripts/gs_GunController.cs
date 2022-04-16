using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gs_GunController : MonoBehaviour
{   
    public gs_GunData m_GunData;
    [Space]

    Script_WeaponAnimatior m_Animator;
    AudioSource AS;

    [Header("Keybinds")]
    [SerializeField] KeyCode ShootKey = KeyCode.Mouse0;
    [SerializeField] KeyCode ReloadKey = KeyCode.R;
    [Space]

    [Header("Firing Point")]
    [SerializeField] Transform FiringPoint;
    [SerializeField] Script_PlayerLook Look;

    [SerializeField] float Recoil;
    [SerializeField] float RecoilTime;
    [SerializeField] float SlerpSpeed;
    [SerializeField] float ShakeTime;
    [SerializeField] float ShakeAmplitude;


    // Gun Variables
    public float m_Damage;
    public float m_Ammo;
    public float m_ShotTimer;
    public float m_SpreadAngle;
    public float m_ShotCount;
    public float m_ProjectileForce;
    public bool m_IsReloading;
    public bool m_CanShoot;

    //FullAutoRampUpStats
    float m_RampUpLerp;
    float m_RampUpTimer;

    //ChargeStats
    public float m_ChargeLerp;
    public float m_ChargeTimer;
    private void Start() 
    {
        FiringPoint = transform.GetChild(0);
        AS = gameObject.GetComponent<AudioSource>();
        m_Animator = GetComponentInChildren<Script_WeaponAnimatior>();
        //Setting Gun Stats
        m_Damage = m_GunData.Damage;
        m_Ammo = m_GunData.AmmoCount;
        m_SpreadAngle = m_GunData.SpreadAngle;
        m_ProjectileForce = m_GunData.ProjectileForce;
        m_ShotCount = m_GunData.ShotCount;
        m_ShotTimer = 0;
    }

    private void OnGUI() 
    {
        //Setting Gun Stats
        m_Damage = m_GunData.Damage;
        m_SpreadAngle = m_GunData.SpreadAngle;
        m_ProjectileForce = m_GunData.ProjectileForce;
        m_ShotCount = m_GunData.ShotCount;
    }

    private void OnEnable() 
    {
        //Setting Gun Stats
        m_Damage = m_GunData.Damage;
        m_Ammo = m_GunData.AmmoCount;
        m_SpreadAngle = m_GunData.SpreadAngle;
        m_ProjectileForce = m_GunData.ProjectileForce;
        m_ShotCount = m_GunData.ShotCount;
        m_ShotTimer = 0;
        m_IsReloading = false;
    }

    

    private void Update() 
    {
        CheckReload();

        m_CanShoot = CanShoot();

        switch (m_GunData.TriggerType)
        {
        case gs_Types.gs_TriggerType.SemiAuto:
            SemiAutoTrigger();
        break;

        case gs_Types.gs_TriggerType.FullAuto:
            FullAutoTrigger();
        break;

        case gs_Types.gs_TriggerType.FullAutoRampUp:
            FullAutoRampUpTrigger();
        break;

        case gs_Types.gs_TriggerType.Burst:
            BurstTrigger();
        break;

        case gs_Types.gs_TriggerType.Charge:
            ChargeTrigger();
        break;

        default:
            Debug.Log("Gun Trigger Selection is Broken");
        break;
        }   
    
    }

    void CheckReload()
    {
        if (m_Ammo < 1 && m_ShotTimer < 1 && !m_IsReloading || m_Ammo < m_GunData.AmmoCount && m_ShotTimer < 1 && !m_IsReloading && Input.GetKey(ReloadKey))
        {
            StartCoroutine(Reload());
            AS.PlayOneShot(m_GunData.ReloadSound);
        }
    }
    private IEnumerator Reload()
    {
        m_IsReloading = true;
        m_Animator.Reload();
        yield return new WaitForSeconds(m_GunData.ReloadTime);
        m_IsReloading = false;
        m_Ammo = m_GunData.AmmoCount;
    }


    private bool CanShoot() // returns true if can shoot else false
    {
        if (m_ShotTimer > 0 || m_IsReloading || m_Ammo < 1)
        {
            m_ShotTimer -= Time.deltaTime;
            return false;
        }
        else
        {
            return true;
        }   
    }

    //Shooting Fucntions
    private void Shoot()
    {
        // Shoots depending on the shot type
        switch (m_GunData.ShotType)
        {
            case gs_Types.gs_ShotType.Hitscan:
            for (int i = 0; i < m_ShotCount; i++)
            {
                ShootHitscan();
            }
            break;

            case gs_Types.gs_ShotType.Projectile:
            for (int i = 0; i < m_ShotCount; i++)
            {
                ShootProjectile();
            }
            break;

            default:
            Debug.LogError("Error with GunData.ShotType");
            break;
        }
            m_Ammo--;
            AS.PlayOneShot(m_GunData.ShootSound);
            m_Animator.Shoot();

            Look.SetShake(ShakeTime, ShakeAmplitude);
            Look.SetRecoil(SlerpSpeed,RecoilTime,Quaternion.Euler(Random.Range(-Recoil/2f, 0),Random.Range(-Recoil/2,Recoil/2),0));
    }

    private void ShootHitscan() // Edit this Section for Hitscan Collisions
    {
        Vector3 HitscanSpread = Vector3.forward;
        HitscanSpread = Quaternion.AngleAxis(Random.Range(0,m_SpreadAngle / 2), Vector3.up) * Vector3.forward;
        HitscanSpread = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward) * HitscanSpread;
        HitscanSpread = FiringPoint.rotation * HitscanSpread;

        RaycastHit hit;
        if (Physics.Raycast(FiringPoint.position, HitscanSpread, out hit, Mathf.Infinity))
        {
            Debug.DrawLine(FiringPoint.position, hit.point, Color.red, 0.1f);
            Debug.Log("hit " + hit.transform.name);
        }
    }

    private void ShootProjectile() // Projectile Collisons are handled on the Projectile otself
    {
        Quaternion ProjectileSpread = FiringPoint.rotation * Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f)) * Quaternion.Euler(Random.Range(0.0f, m_SpreadAngle / 2), 0, 0);

        GameObject Proj = Instantiate(m_GunData.Projectile, FiringPoint.position, ProjectileSpread);
        Rigidbody RB = Proj.GetComponent<Rigidbody>();
        RB.useGravity = m_GunData.ApplyGravity;
        RB.AddForce(Proj.transform.forward * m_ProjectileForce, ForceMode.VelocityChange);
        GameObject.Destroy(Proj, m_GunData.ProjectileLifetime);
    }

    //TriggerType Functions

    private void SemiAutoTrigger()
    {
        if (m_CanShoot)
        {
            if (Input.GetKeyDown(ShootKey))
            {
                Shoot();
                m_ShotTimer = m_GunData.FireRate;
            }
        }
    }
    private void FullAutoTrigger()
    {
        if (m_CanShoot)
        {
            if (Input.GetKey(ShootKey))
            {
                Shoot();
                m_ShotTimer = m_GunData.FireRate;
            }
        }
    }

    private void FullAutoRampUpTrigger()
    {
        if (Input.GetKey(ShootKey) && !m_IsReloading)
        {
            if (m_RampUpTimer < m_GunData.RampUpTime)
            {
                m_RampUpTimer += Time.deltaTime; 
            }
            else
            {
                m_RampUpTimer = m_GunData.RampUpTime;
            }

            if (m_CanShoot)
            {
                m_RampUpLerp = m_RampUpTimer / m_GunData.RampUpTime;
                m_ShotTimer = Mathf.Lerp(m_GunData.FireRate, m_GunData.MaxFireRate, m_RampUpLerp);
                Shoot();
            }
        }
        else
        {
            m_RampUpTimer = 0;
        }
    }

    private void BurstTrigger()
    {
        if (m_CanShoot)
        {
            if (Input.GetKeyDown(ShootKey))
            {
                for (int i = 0; i < m_GunData.BurstCount; i++)
                {
                    StartCoroutine(Burst(m_GunData.BurstDelay * i));
                }
            }
        }
    }

    private IEnumerator Burst(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Burst Shot");
        if (m_Ammo > 0)
        {
            Shoot();
            m_Ammo--;
            m_ShotTimer = m_GunData.FireRate;
        }
    }

    private void ChargeTrigger()
    {
        if (Input.GetKey(ShootKey) && !m_IsReloading)
        {
            if (m_ChargeTimer < m_GunData.ChargeTime)
            {
                m_ChargeTimer += Time.deltaTime;
                
            }
            else
            {
                m_ChargeTimer = m_GunData.ChargeTime;
            }
        }

        m_ChargeLerp = m_ChargeTimer / m_GunData.ChargeTime;

        if (m_CanShoot)
        {
            if (Input.GetKeyUp(ShootKey))
            {
                switch (m_GunData.ChargeType)
                {
                    case gs_Types.gs_ChargeType.Damage:
                        m_Damage = Mathf.Lerp(m_GunData.Damage, m_GunData.MaxDamage, m_ChargeLerp);
                        Shoot();
                    break;

                    case gs_Types.gs_ChargeType.Accuracy:
                        m_SpreadAngle = Mathf.Lerp(m_GunData.SpreadAngle, m_GunData.MinSpreadAngle, m_ChargeLerp);
                        Shoot();
                    break;

                    case gs_Types.gs_ChargeType.ShotCount:
                        m_ShotCount = (int)Mathf.Lerp(m_GunData.ShotCount, m_GunData.MaxShotCount, m_ChargeLerp);
                        Shoot();
                    break;

                    case gs_Types.gs_ChargeType.Burst: 
                        int count  = (int)Mathf.Lerp(0, m_GunData.BurstCount, m_ChargeLerp);
                        for (int i = 0; i < count; i++)
                        {
                            StartCoroutine(Burst(m_GunData.BurstDelay * i));
                        }
                    break;

                    case gs_Types.gs_ChargeType.Force:
                        m_ProjectileForce = Mathf.Lerp(m_GunData.ProjectileForce, m_GunData.MaxProjectileForce, m_ChargeLerp);
                        Shoot();
                    break;
                        
                    default:
                    Debug.LogError("Error with m_GunData.ChargeType");
                    break;
                }
                m_ShotTimer = m_GunData.FireRate;
                m_ChargeTimer = 0;
            }
        }
    }
}


