using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerStatManager : MonoBehaviour
{
    //Singleton Pattern
    public static Script_PlayerStatManager Instance;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Debug.Log("Damagepopup Manager fucked up");
            Destroy(gameObject);
        }
    }

    
    [System.Serializable]
    public struct WeaponStats {
        [SerializeField]
        public string WeaponName;
        [SerializeField]
        public float Default_Damage;
        [SerializeField]
        public float Modified_Damage;
        [SerializeField]
        public int Default_MaxAmmo;
        [SerializeField]
        public int Modified_MaxAmmo;
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

    [Header("Stat Caps")]
    public float FirerateCap = 0.25f;
    public float DamageCap = 100f;

    public void SetWeaponStats(int weaponIndex, Script_WeaponBase _weaponBase)
    {
        WeaponStats weapon = WeaponStatList.ToArray()[weaponIndex];
        weapon.WeaponName = _weaponBase.GetGunName();
        weapon.Default_Damage = _weaponBase.GetDamage();
        weapon.Default_Firerate = _weaponBase.GetFireRate();
        weapon.Default_MaxAmmo = _weaponBase.GetMagCount();

        weapon.Modified_Damage = _weaponBase.GetDamage();
        weapon.Modified_Firerate = _weaponBase.GetFireRate();
        weapon.Modified_MaxAmmo = _weaponBase.GetMagCount();

        WeaponStatList[weaponIndex] = weapon;
    }

    public void SetModifiedDamage(Grid SelectGrid, float _damageIncrease, bool _isAdditive)
    {
        if (SelectGrid.WeaponIndex > 3) return;
        WeaponStats weapon = WeaponStatList.ToArray()[SelectGrid.WeaponIndex];
        if (_isAdditive)
        {
            weapon.Modified_Damage += _damageIncrease;
        }
        else {
            weapon.Modified_Damage -= _damageIncrease;
        }
        WeaponStatList[SelectGrid.WeaponIndex] = weapon;
    }

    public void SetModifiedFireRate(Grid SelectGrid, float _firerateIncrease, bool _isAdditive)
    {
        if (SelectGrid.WeaponIndex > 3) return;
        WeaponStats weapons = WeaponStatList.ToArray()[SelectGrid.WeaponIndex];

        if (_isAdditive)
        {
            weapons.Modified_Firerate += _firerateIncrease;
            
        }
        else
        {
            weapons.Modified_Firerate -= _firerateIncrease;
        }

        WeaponStatList[SelectGrid.WeaponIndex] = weapons;  
    }
    public void SetModifiedMaxAmmo(int weaponIndex, int _ammoIncrease, bool _isAdditive)
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





}

