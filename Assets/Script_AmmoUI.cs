using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Script_AmmoUI : MonoBehaviour
{
    [SerializeField] Script_PlayerWeapons currentWeapon;
    public TMP_Text m_maxAmmo;
    public TMP_Text m_currentAmmo;

    public void OnAmmoChange(int _ammo)
    {
        m_currentAmmo.text = _ammo.ToString();
    }

    // Start is called before the first frame update
    void Awake()
    {
        currentWeapon = GetComponentInParent<Script_PlayerWeapons>();
        currentWeapon.getCurrentEquip().onAmmoChangeEvent += OnAmmoChange;
        m_maxAmmo.text = currentWeapon.getCurrentEquip().GetMagCount().ToString();
        m_currentAmmo.text = m_maxAmmo.text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
