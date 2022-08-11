using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using System;

public class Scr_UpgradeShop : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject UpgradeShop_Canvas;
    [Space]
    public bool isShopActive = false;

    public CinemachineVirtualCamera vcam;

   

    [System.Serializable]
    public struct ShopWeaponUI
    {
        public TMP_Text GunName;
        public TMP_Text Damage;
        public TMP_Text Cost;
        public TMP_Text magazineSize;

    }

    public List<ShopWeaponUI> ShopSlots;

    public void EnableShopCanvas(bool _b)
    {
        UpgradeShop_Canvas.SetActive(_b);
        isShopActive = _b;
        if (_b == true)
        {
            vcam.Priority = 10;
        }
        else {
            vcam.Priority = 9;
        }
    }
    private void Init()
    {
        int i = 0; 
        foreach (Script_PlayerStatManager.WeaponStats weapon in Script_PlayerStatManager.Instance.WeaponStatList)
        {
            /*Script_PlayerStatManager.WeaponStats weaponStats = Script_PlayerStatManager.Instance.WeaponStatList.ToArray()[i];*/
            SetWeaponShopSlot(weapon, i);
            i++;
        }
    }

    private void SetWeaponShopSlot(Script_PlayerStatManager.WeaponStats weaponStats,int i)
    {
        ShopSlots.Add(new ShopWeaponUI());

        ShopSlots[i].GunName.text = weaponStats.WeaponName;
        ShopSlots[i].Damage.text = weaponStats.Modified_Damage.ToString();
        ShopSlots[i].magazineSize.text = weaponStats.Modified_MaxAmmo.ToString();

        ShopSlots[i].Cost.text = "100";
    }


    private void Start()
    { 
        if (UpgradeShop_Canvas == null)
        {
            Debug.LogWarning("Missing UpgradeShopCanvas");
        }
        Init();

        
    }

    private void Update()
    {
        if (isShopActive && Input.GetKeyDown(KeyCode.Tab))
        {
            EnableShopCanvas(false);
        }
    }
}
