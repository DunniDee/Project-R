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
    [System.Serializable]
    public struct WeaponStats {
        [SerializeField]
        public float Default_Damage;
        [SerializeField]
        public float Modified_Damage;
        [SerializeField]
        public int Default_MaxAmmo;
        [SerializeField]
        public float Modified_MaxAmmo;
        [SerializeField]
        public float Default_Firerate;
        [SerializeField]
        public float Modified_Firerate;
    }

    // Weapon Stats
    public List<WeaponStats> WeaponStatList;

    [Header("Base Variables")]
    public float Default_MaxHealth;
    public float Modified_MaxHealth;


    [Header("Bounty variables")]
    public float Bounty;
    public float Credits;


    public void SetWeaponStats(int weaponIndex, Script_WeaponBase _weaponBase)
    {
        WeaponStats weapon = WeaponStatList.ToArray()[weaponIndex];
        weapon.Default_Damage = _weaponBase.GetDamage();
        weapon.Default_Firerate = _weaponBase.GetFireRate();
        weapon.Default_MaxAmmo = _weaponBase.GetMagCount();

        weapon.Modified_Damage = _weaponBase.GetDamage();
        weapon.Modified_Firerate = _weaponBase.GetFireRate();
        weapon.Modified_MaxAmmo = _weaponBase.GetMagCount();

        WeaponStatList[weaponIndex] = weapon;
    }

    public void SetModifiedDamage(int weaponIndex, float _damageIncrease, bool _isAdditive)
    {
        WeaponStats weapon = WeaponStatList.ToArray()[weaponIndex];
        if (_isAdditive)
        {
            weapon.Modified_Damage += _damageIncrease;
        }
        else {
            weapon.Modified_Damage -= _damageIncrease;
        }
        WeaponStatList[weaponIndex] = weapon;
    }

    public void SetModifiedFireRate(int weaponIndex, float _firerateIncrease, bool _isAdditive)
    {
        WeaponStats weapon = WeaponStatList.ToArray()[weaponIndex];
        if (_isAdditive)
        {
            weapon.Default_Firerate += _firerateIncrease;
        }
        else
        {
            weapon.Default_Firerate -= _firerateIncrease;
        }
        WeaponStatList[weaponIndex] = weapon;
    }

    public void SetModifiedMaxAmmo(int weaponIndex, float _ammoIncrease, bool _isAdditive)
    {
        WeaponStats weapon = WeaponStatList.ToArray()[weaponIndex];
        if (_isAdditive)
        {
            weapon.Modified_MaxAmmo += _ammoIncrease;
        }
        else
        {
            weapon.Modified_MaxAmmo -= _ammoIncrease;
        }
        WeaponStatList[weaponIndex] = weapon;
    }
    public void Start()
    {
        
    }




}

