using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHighlight : MonoBehaviour
{
    [SerializeField] RectTransform highlighter;

    public void Show(bool _b)
    {
        highlighter.gameObject.SetActive(_b);
    }

    public void SetSize(InventoryItem _targetItem)
    {
        Vector2 size = new Vector2();
        size.x = _targetItem.itemData.width * Grid.tileSizeWidth;
        size.y = _targetItem.itemData.height * Grid.tileSizeHeight;

        highlighter.sizeDelta = size;
    }

    public void SetPosition(Grid _targetGrid, InventoryItem _targetitem)
    {
        SetParent(_targetGrid);

        Vector2 Position = _targetGrid.CalculatePositionOnGrid(_targetitem, _targetitem.ongridPositionX, _targetitem.ongridPositionY);

        highlighter.localPosition = Position;
    }
    public void SetPosition(Grid _targetGrid, InventoryItem _targetitem, int _posX, int _posY)
    {
        Vector2 Position = _targetGrid.CalculatePositionOnGrid(_targetitem, _posX, _posY);

        highlighter.localPosition = Position;
    }

    public void SetParent(Grid _targetGrid)
    {
        if (_targetGrid == null)
        { 
            return;
        }
        highlighter.SetParent(_targetGrid.GetComponent<RectTransform>());
    }


    
}
