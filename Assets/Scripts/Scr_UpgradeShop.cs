using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scr_UpgradeShop : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject UpgradeShop_Canvas;
    [Space]
    public bool isShopActive = false;

    public void EnableShopCanvas(bool _b)
    {
        UpgradeShop_Canvas.SetActive(_b);
        isShopActive = _b;
    }
    
    [System.Serializable]
    public struct ShopWeaponUI
    {
        public TMP_Text GunName;
        public TMP_Text Damage;
        public TMP_Text Cost;
        public TMP_Text magazineSize;

    }
    public List<ShopWeaponUI> ShopSlots;

    private void Start()
    { 
        if (UpgradeShop_Canvas == null)
        {
            Debug.LogWarning("Missing UpgradeShopCanvas");
        }
     

        
    }

    private void Update()
    {
        if (isShopActive)
        {
            EnableShopCanvas(false);
        }
    }
}
