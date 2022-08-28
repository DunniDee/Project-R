using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public ItemData itemData;

    public int ongridPositionX;
    public int ongridPositionY;

    public bool isItemEquppied = false;
    public delegate void UpdateGunStatsDelegate();
    public event UpdateGunStatsDelegate UpdateGunStatsEvent;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_selectedGrid"></param>
    public void Equip(Grid _selectedGrid)
    {
        if (isItemEquppied == false)
        {
            isItemEquppied = true;
            Script_PlayerStatManager.Instance.SetModifiedDamage(_selectedGrid, itemData.DamageIncrease, true);
            Script_PlayerStatManager.Instance.SetModifiedFireRate(_selectedGrid, itemData.FireRateIncrease, false);
            UpdateGunStats();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void UpdateGunStats()
    {
        if (UpdateGunStatsEvent != null)
        {
            UpdateGunStatsEvent();
        }
        else
        {
            Debug.LogWarning("Failed to update GunStats... Event Is null");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_selectedGrid"></param>
    public void Dequip(Grid _selectedGrid)
    {
        if (isItemEquppied == true)
        {
            isItemEquppied = false;

            
            Script_PlayerStatManager.Instance.SetModifiedFireRate(_selectedGrid, itemData.FireRateIncrease, true);
            Script_PlayerStatManager.Instance.SetModifiedDamage(_selectedGrid, itemData.DamageIncrease, false);
            UpdateGunStats();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="itemData"></param>
    internal void Set(ItemData itemData)
    {
        this.itemData = itemData;

        GetComponent<Image>().sprite = itemData.itemIcon;

        Vector2 size = new Vector2();
        size.x = itemData.width * Grid.tileSizeWidth;
        size.y = itemData.height * Grid.tileSizeHeight;

        GetComponent<RectTransform>().sizeDelta = size;
    }

    public void Start()
    {
        UpdateGunStatsEvent += script_WeaponSwap.Instance.UpdateWeaponStats;
    }
}
