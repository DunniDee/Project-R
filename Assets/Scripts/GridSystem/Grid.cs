using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public enum GridType
    {
        EQUIPPED,
        INVENTORY,
        STATIC
    }
    public GridType gridType = GridType.STATIC;

    public const float tileSizeWidth = 32;
    public const float tileSizeHeight = 32;

    InventoryItem[,] inventoryItemSlot;

    RectTransform rectTransform;

    [SerializeField] int gridSizeWidth = 8;
    [SerializeField] int gridSizeHeight = 10;

    [SerializeField] GameObject abilityItemPrefab;
    private void OnValidate()
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
            Init(gridSizeWidth, gridSizeHeight);
        }
        
    }
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(gridSizeWidth, gridSizeHeight);

    }

    public InventoryItem PickUpItem(int x, int y)
    {
        InventoryItem r = inventoryItemSlot[x, y];
        if (r == null) { return null; }

        CleanupItemSlots(r);

        return r;
    }

    private void CleanupItemSlots(InventoryItem item)
    {
        for (int i = 0; i < item.itemData.width; i++)
        {
            for (int j = 0; j < item.itemData.height; j++)
            {
                inventoryItemSlot[item.ongridPositionX + i, item.ongridPositionY + j] = null;
            }
        }
    }



    private void Init(int _width, int _height)
    {
        inventoryItemSlot = new InventoryItem[_width, _height];
        Vector2 size = new Vector2(_width * tileSizeWidth, _height * tileSizeHeight);
        rectTransform.sizeDelta = size;
    }

    internal InventoryItem GetItem(int x, int y)
    {
        return inventoryItemSlot[x, y];
    }

    Vector2 PositionOnTheGrid = new Vector2();
    Vector2Int tileGridPosition = new Vector2Int();

    public Vector2Int GetGridPosition(Vector2 _mousePos)
    {
        PositionOnTheGrid.x = _mousePos.x - rectTransform.position.x;
        PositionOnTheGrid.y = rectTransform.position.y - _mousePos.y;

        tileGridPosition.x = (int)(PositionOnTheGrid.x / tileSizeWidth);
        tileGridPosition.y = (int)(PositionOnTheGrid.y / tileSizeWidth);

        return tileGridPosition;
    }

    public bool PlaceItem(InventoryItem _inventoryItem, int _posX,int _posY, ref InventoryItem overlapItem)
    {
        if (BoundryCheck(_posX, _posY, _inventoryItem.itemData.width, _inventoryItem.itemData.height) == false)
        {
            return false;
        }

        if (OverlapCheck(_posX, _posY, _inventoryItem.itemData.width, _inventoryItem.itemData.height, ref overlapItem) == false)
        {
            overlapItem = null;
            return false;
        }
        if (overlapItem != null)
        {
            CleanupItemSlots(overlapItem);
        }

        RectTransform rectTransform = _inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);
        for (int i = 0; i < _inventoryItem.itemData.width; i++)
        {
            for (int y = 0; y < _inventoryItem.itemData.height; y++)
            {
                inventoryItemSlot[_posX + i, _posY + y] = _inventoryItem;
            }
        }

        _inventoryItem.ongridPositionX = _posX;
        _inventoryItem.ongridPositionY = _posY;
        Vector2 Position = CalculatePositionOnGrid(_inventoryItem, _posX, _posY);

        rectTransform.localPosition = Position;
        return true;
    }

    public Vector2 CalculatePositionOnGrid(InventoryItem _inventoryItem, int _posX, int _posY)
    {
        Vector2 Position = new Vector2();
        Position.x = _posX * tileSizeWidth + tileSizeWidth * _inventoryItem.itemData.width / 2;
        Position.y = -(_posY * tileSizeHeight + tileSizeHeight * _inventoryItem.itemData.height / 2);
        return Position;
    }

    private bool OverlapCheck(int posX, int posY, int width, int height, ref InventoryItem overlapItem)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (inventoryItemSlot[posX + i, posY + j] != null)
                {
                    if (overlapItem == null)
                    {
                        overlapItem = inventoryItemSlot[posX + i, posY + j];
                    }
                    else {
                        if (overlapItem != inventoryItemSlot[posX + i, posY + j])
                        {
                            return false;
                        }
                        
                    }
                    
                }
            }
        }
        return true;
    }

    public void ActivateItems(bool _b)
    {
        for (int i = 0; i < gridSizeWidth; i++)
        {
            for (int j = 0; j < gridSizeWidth; j++)
            {
                if (inventoryItemSlot[i, j] != null)
                {
                    inventoryItemSlot[i, j].isItemEquppied = _b;
                }
                
            }
        }
    }

    public GridType CheckGridType()
    {
        switch (gridType)
        {
            case GridType.INVENTORY:
                return GridType.INVENTORY;
            case GridType.EQUIPPED:
                return GridType.EQUIPPED;
            default:
                return GridType.STATIC;
        }
    }
    bool PositionCheck(int _x, int _y)
    {
        if (_x < 0 || _y < 0)
        {
            return false;
        }
        if (_x >= gridSizeWidth || _y >= gridSizeHeight)
        {
            return false;
        }

        return true;
    }

    public bool BoundryCheck(int _x, int _y, int _sizeX, int _sizeY)
    {
        if (PositionCheck(_x, _y) == false) return false;

        _x += _sizeX - 1;
        _y += _sizeY - 1;

        if (PositionCheck(_x, _y) == false) return false;

        return true;
    }
}
