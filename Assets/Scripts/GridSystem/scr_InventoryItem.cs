using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_InventoryItem : MonoBehaviour
{
    public ItemData itemData;

    public int ongridPositionX;
    public int ongridPositionY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void Set(ItemData itemData)
    {
        this.itemData = itemData;

        GetComponent<Image>().sprite = itemData.itemIcon;

        Vector2 size = new Vector2();
        size.x = itemData.width * scr_AbilityGrid.tileSizeWidth;
        size.y = itemData.height * scr_AbilityGrid.tileSizeHeight;

        GetComponent<RectTransform>().sizeDelta = size;
    }
}
