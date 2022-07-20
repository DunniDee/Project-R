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

    public void Equip()
    {
        if (isItemEquppied == false)
        {
            isItemEquppied = true;
        }
    }

    public void Dequip()
    {
        if (isItemEquppied == true)
        {
            isItemEquppied = false;
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
