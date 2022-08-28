using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Script Owner - Ashley Rickit


public class Grid : MonoBehaviour
{
    public enum GridType
    {
        EQUIPABILITY,
        INVENTORY,
        EQUIPWEAPON,
        STATIC
    }
    [Header("Grid Properties")]
    public const float tileSizeWidth = 32;
    public const float tileSizeHeight = 32;

    [SerializeField] int m_gridSizeWidth = 8;
    [SerializeField] int m_gridSizeHeight = 10;

    public GridType gridType = GridType.STATIC;

    Vector2 m_PositionOnTheGrid = new Vector2();
    Vector2Int m_tileGridPosition = new Vector2Int();

    [SerializeField] GameObject m_abilityItemPrefab;

    //Weapon Index to modify stats of.
    [Range(0, 3)]
    public int WeaponIndexToEdit = 0;

    //Disabled Grid Color
    public Color StaticColor;

    InventoryItem[,] m_inventoryItemSlot;

    RectTransform m_rectTransform;

    /// <summary>
    /// 
    /// </summary>
    private void OnValidate()
    {
        if (m_rectTransform == null)
        {
            m_rectTransform = GetComponent<RectTransform>();
            Init(m_gridSizeWidth, m_gridSizeHeight);
        }
        
    }

    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        m_rectTransform = GetComponent<RectTransform>();
        Init(m_gridSizeWidth, m_gridSizeHeight);
    }

    /// <summary>
    /// Return Inventory Item from grid position x,y and remove the item from the grid list.
    /// </summary>
    /// <param name="x"> X Poisition of the Item to Pick up </param>
    /// <param name="y"> Y Poisition of the Item to Pick up</param>
    /// <returns></returns>
    public InventoryItem PickUpItem(int x, int y)
    {
        InventoryItem r = m_inventoryItemSlot[x, y];
        if (r == null) { return null; }

        CleanupItemSlots(r);

        return r;
    }

    /// <summary>
    /// Remove item from InventoryItemSlot list.
    /// </summary>
    /// <param name="item"></param>
    public void CleanupItemSlots(InventoryItem item)
    {
        for (int i = 0; i < item.itemData.width; i++)
        {
            for (int j = 0; j < item.itemData.height; j++)
            {
                m_inventoryItemSlot[item.ongridPositionX + i, item.ongridPositionY + j] = null;
            }
        }
    }

    /// <summary>
    /// Initalise the Grid.
    /// </summary>
    /// <param name="_width"> Width of the Grid</param>
    /// <param name="_height"> Height of the grid</param>
    private void Init(int _width, int _height)
    {
        m_inventoryItemSlot = new InventoryItem[_width, _height];
        Vector2 size = new Vector2(_width * tileSizeWidth, _height * tileSizeHeight);
        m_rectTransform.sizeDelta = size;
        if (gridType == GridType.STATIC)
        {
            GetComponent<UnityEngine.UI.Image>().color = StaticColor;
            
        }
    }

    /// <summary>
    /// Return item at position, x,y.
    /// </summary>
    /// <param name="_x"> Position x of cursor on the grid </param>
    /// <param name="_y"> Position y of cursor on the grid</param>
    /// <returns> Inventory Item at XY </returns>
    internal InventoryItem GetItem(int _x, int _y)
    {
        return m_inventoryItemSlot[_x, _y];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_mousePos"></param>
    /// <returns></returns>
    public Vector2Int GetGridPosition(Vector2 _mousePos)
    {
        m_PositionOnTheGrid.x = _mousePos.x - m_rectTransform.position.x;
        m_PositionOnTheGrid.y = m_rectTransform.position.y - _mousePos.y;

        m_tileGridPosition.x = (int)(m_PositionOnTheGrid.x / tileSizeWidth);
        m_tileGridPosition.y = (int)(m_PositionOnTheGrid.y / tileSizeWidth);

        return m_tileGridPosition;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_inventoryItem"></param>
    /// <param name="_posX"></param>
    /// <param name="_posY"></param>
    /// <param name="overlapItem"></param>
    /// <returns></returns>
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
        rectTransform.SetParent(this.m_rectTransform);
        for (int i = 0; i < _inventoryItem.itemData.width; i++)
        {
            for (int y = 0; y < _inventoryItem.itemData.height; y++)
            {
                m_inventoryItemSlot[_posX + i, _posY + y] = _inventoryItem;
            }
        }

        _inventoryItem.ongridPositionX = _posX;
        _inventoryItem.ongridPositionY = _posY;
        Vector2 Position = CalculatePositionOnGrid(_inventoryItem, _posX, _posY);

        rectTransform.localPosition = Position;
        rectTransform.localScale = Vector3.one;
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_inventoryItem"></param>
    /// <param name="_posX"></param>
    /// <param name="_posY"></param>
    /// <returns></returns>
    public Vector2 CalculatePositionOnGrid(InventoryItem _inventoryItem, int _posX, int _posY)
    {
        Vector2 Position = new Vector2();
        Position.x = _posX * tileSizeWidth + tileSizeWidth * _inventoryItem.itemData.width / 2;
        Position.y = -(_posY * tileSizeHeight + tileSizeHeight * _inventoryItem.itemData.height / 2);
        return Position;
    }

    /// <summary>
    /// Check if cursor position x,y is overlapping with the currently selected item.
    /// </summary>
    /// <param name="_posX"></param>
    /// <param name="_posY"></param>
    /// <param name="_width"></param>
    /// <param name="height"></param>
    /// <param name="_overlapItem"></param>
    /// <returns></returns>
    private bool OverlapCheck(int _posX, int _posY, int _width, int _height, ref InventoryItem _overlapItem)
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                if (m_inventoryItemSlot[_posX + i, _posY + j] != null)
                {
                    if (_overlapItem == null)
                    {
                        _overlapItem = m_inventoryItemSlot[_posX + i, _posY + j];
                    }
                    else {
                        if (_overlapItem != m_inventoryItemSlot[_posX + i, _posY + j])
                        {
                            return false;
                        }
                        
                    }
                    
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Loop over all items and set "isEquipped" to _b
    /// </summary>
    /// <param name="_b"></param>
    public void ActivateItems(bool _b)
    {
        for (int i = 0; i < m_gridSizeWidth; i++)
        {
            for (int j = 0; j < m_gridSizeWidth; j++)
            {
                if (m_inventoryItemSlot[i, j] != null)
                {
                    m_inventoryItemSlot[i, j].isItemEquppied = _b;
                }
                
            }
        }
    }

    /// <summary>
    /// Returns current grid type.
    /// </summary>
    /// <returns></returns>
    public GridType CheckGridType()
    {
        switch (gridType)
        {
            case GridType.INVENTORY:
                return GridType.INVENTORY;
            case GridType.EQUIPABILITY:
                return GridType.EQUIPABILITY;
            default:
                return GridType.STATIC;
        }
    }

    /// <summary>
    /// Check if the Position at x,y is avaliable.
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
    /// <returns></returns>
    bool PositionCheck(int _x, int _y)
    {
        if (_x < 0 || _y < 0)
        {
            return false;
        }
        if (_x >= m_gridSizeWidth || _y >= m_gridSizeHeight)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Check if the position and size are within the grid.
    /// </summary>
    /// <param name="_x"></param>
    /// <param name="_y"></param>
    /// <param name="_sizeX"></param>
    /// <param name="_sizeY"></param>
    /// <returns></returns>
    public bool BoundryCheck(int _x, int _y, int _sizeX, int _sizeY)
    {
        if (PositionCheck(_x, _y) == false) return false;

        _x += _sizeX - 1;
        _y += _sizeY - 1;

        if (PositionCheck(_x, _y) == false) return false;

        return true;
    }
}
