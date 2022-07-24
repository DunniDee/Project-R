using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType { 
    
}
public class InventoryItem : MonoBehaviour
{
    public ItemData itemData;

    public int ongridPositionX;
    public int ongridPositionY;

    public bool isItemEquppied = false;
    public delegate void UpdateGunStatsDelegate();
    public event UpdateGunStatsDelegate UpdateGunStatsEvent;

    public void Equip()
    {
        if (isItemEquppied == false)
        {
            isItemEquppied = true;
            Script_PlayerStatManager.Instance.SetModifiedDamage(0, itemData.DamageIncrease, true);
            UpdateGunStats();
        }
    }

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

    public void Dequip()
    {
        if (isItemEquppied == true)
        {
            isItemEquppied = false;
            Script_PlayerStatManager.Instance.SetModifiedDamage(0, itemData.DamageIncrease, false);
            UpdateGunStats();
        }
    }
    internal void Set(ItemData itemData)
    {
        this.itemData = itemData;

        GetComponent<Image>().sprite = itemData.itemIcon;

        Vector2 size = new Vector2();
        size.x = itemData.width * Grid.tileSizeWidth;
        size.y = itemData.height * Grid.tileSizeHeight;

        GetComponent<RectTransform>().sizeDelta = size;
    }
}
