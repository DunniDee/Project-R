using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "ItemData",menuName = "ItemData")]
public class ItemData : ScriptableObject
{
    public int width = 1;
    public int height = 1;

    public Sprite itemIcon;

    public string itemDescription;

    public int DamageIncrease = 2;
    public int SellValue;
}
