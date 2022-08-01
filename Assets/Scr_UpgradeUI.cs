using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scr_UpgradeUI : MonoBehaviour
{
    public TMP_Text Description;

    [System.Serializable]
    public struct WeaponUI {
        public TMP_Text Header;
        public TMP_Text Damage;
        public TMP_Text Firerate;
        public TMP_Text MagCount;
    }

    public WeaponUI Weapon1;
    public WeaponUI Weapon2;
    public WeaponUI Weapon3;
    public WeaponUI Weapon4;

    public void Awake()
    {
        
    }

    public void SetWeaponUIElements()
    {
        SetWeaponUIElement(Weapon1,0);
        SetWeaponUIElement(Weapon2,1);
        SetWeaponUIElement(Weapon3,2);
        SetWeaponUIElement(Weapon4,3);
    }

    public void SetWeaponUIElement(WeaponUI weaponUI, int weaponIndex)
    {
        Script_PlayerStatManager.WeaponStats weaponStats = Script_PlayerStatManager.Instance.WeaponStatList.ToArray()[0];
        weaponUI.Header.text = weaponStats.WeaponName;
        weaponUI.Damage.text = weaponStats.Modified_Damage.ToString();
        weaponUI.Firerate.text = weaponStats.Modified_Firerate.ToString();

        Debug.Log("Updating UI Elements...");
    }
    public void Update()
    {
        
    }
}
