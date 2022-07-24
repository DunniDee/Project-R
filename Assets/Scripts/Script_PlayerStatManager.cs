using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerStatManager : MonoBehaviour
{
    //Singleton Pattern
    public static Script_PlayerStatManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        } else {
            Instance = this;
          	DontDestroyOnLoad(gameObject);
        }
    }
    public struct WeaponStats {
        public float Default_Damage { get; set; }
        public float Modified_Damage { get; set; }
        public int Default_MaxAmmo { get; set; }
        public float Modified_MaxAmmo { get; set; }

        public float Default_Firerate { get; set; }
        public float Modified_Firerate { get; set; }
    }

    // Weapon Stats
    public WeaponStats RevolverStats;
    public WeaponStats SMGStats;
    public WeaponStats PistolStats;
    public WeaponStats MinigunStats;
    public WeaponStats RailGunStats;
    public WeaponStats LauncherStats;

    public void SetWeaponStats(WeaponStats weaponStats, Script_WeaponBase _weaponBase)
    {
        if (_weaponBase as Weapon_SemiAutioProjectile)
        {

        }
        else if (_weaponBase as Weapon_RampUpProjectile)
        { 
            
        }
        else if (_weaponBase as Weapon_FullAutoProjectile)
        {

        }
    }

    public void Start()
    {
        
    }
    [Header("Bounty variables")]
    public float Bounty;
    public float Credits;


}

