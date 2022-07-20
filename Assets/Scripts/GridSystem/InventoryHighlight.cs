using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHighlight : MonoBehaviour
{
    [SerializeField] RectTransform highlighter;

    public void Show(bool _b)
    {
        highlighter.gameObject.SetActive(_b);
    }
    public void SetSize(scr_InventoryItem targetItem)
    {
        Vector2 size = new Vector2();
        size.x = targetItem.itemData.width * scr_AbilityGrid.tileSizeWidth;
        size.y = targetItem.itemData.height * scr_AbilityGrid.tileSizeHeight;

        highlighter.sizeDelta = size;

    }

    public void SetPosition(scr_AbilityGrid targetGrid, scr_InventoryItem _targetitem)
    {
        SetParent(targetGrid);

        Vector2 pos = targetGrid.CalculatePositionOnGrid(_targetitem, _targetitem.ongridPositionX, _targetitem.ongridPositionY);

        highlighter.localPosition = pos;

    }

    public void SetParent(scr_AbilityGrid targetGrid)
    {
        if (targetGrid == null)
        { 
            return;
        }
        highlighter.SetParent(targetGrid.GetComponent<RectTransform>());
    }

    public void SetPosition(scr_AbilityGrid targetGrid, scr_InventoryItem _targetitem, int posX, int posY)
    {
        Vector2 pos = targetGrid.CalculatePositionOnGrid(_targetitem, posX, posY);

        highlighter.localPosition = pos;

    }
    
}
