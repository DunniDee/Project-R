/*****************************************************************/
/* NAME: Ashley Rickit */
/* ORGN: Greeble */
/* FILE: GridController.cs */
/* DATE:  */
/*****************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutController: MonoBehaviour
{
    public static LoadoutController i;

    [HideInInspector]
    private Grid m_selectedGrid;
    public Grid SelectedItemGrid { 
        get => m_selectedGrid;
        set {
            m_selectedGrid = value;
            gridHighlight.SetParent(value);
        }
    }

    public InventoryItem m_selectedItem;
    InventoryItem m_overlapItem;

    Vector2Int m_oldPositionOnGrid;
    InventoryItem m_itemToHighlight;

    public RectTransform m_rectTransform;

    public List<ItemData> m_itemDataList;
    [SerializeField] GameObject m_itemPrefab;
    [SerializeField] GameObject m_UIitemPrefab;
    public Transform m_canvasTransform;

    public Transform GetUICanvas()
    {
        return m_canvasTransform;
    }
    GridHighlight gridHighlight;
    [SerializeField]Scr_UpgradeUI UpgradeUI;

    public AudioSource audioSource;
    public AudioClip pickupNoise;
    public AudioClip dropNoise;

    bool isGridActive = false;

    #region Getters & Setters
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool GetIsGridActive() { return isGridActive; }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_b"></param>
    public void SetIsGridActive(bool _b) { isGridActive = _b; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_Fullscreen"></param>
    public void SetFullScreen(bool _Fullscreen)
    {
        Screen.fullScreen = _Fullscreen;
    }

    /// <summary>
    /// /
    /// </summary>
    /// <param name="_graphicsIndex"></param>
    public void SetQuality(int _graphicsIndex)
    {
        QualitySettings.SetQualityLevel(_graphicsIndex);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_Audio"></param>
    public void SetMasterAudio(float _Audio)
    {
        AudioListener.volume = _Audio;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public ItemData GetSelectedItemData()
    {
        return m_selectedItem.itemData;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_camera"></param>
    /// <param name="_worldDepth"></param>
    /// <returns></returns>
    public Vector3 GetMouseWorldPosition(Camera _camera, float _worldDepth)
    {
        Vector2 screenPosition = Input.mousePosition;
        Vector3 screenPositionWithDepth = new Vector3(screenPosition.x, screenPosition.y, _worldDepth);
        Debug.Log(_camera.ScreenToWorldPoint(screenPositionWithDepth));
        return _camera.ScreenToWorldPoint(screenPositionWithDepth);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_b"></param>
    public void SetUIActive(bool _b)
    {
        SetCursorActive(_b);

        SetIsGridActive(_b);
        m_canvasTransform.gameObject.SetActive(_b);
        UpgradeUI.enabled = _b;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_b"></param>
    public void SetCursorActive(bool _b)
    {
        FindObjectOfType<Scr_PlayerMotor>().enabled = !_b;
        FindObjectOfType<Scr_PlayerLook>().enabled = !_b;
        FindObjectOfType<Scr_PlayerHealth>().enabled = !_b;
        script_WeaponSwap.Instance.SetCanShoot(!_b);

        Cursor.visible = _b;
        Cursor.lockState = Cursor.visible ? CursorLockMode.Confined : CursorLockMode.Locked;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Vector2Int GetTileGridPosition()
    {
        Vector2 Position = Input.mousePosition;
        if (m_selectedItem != null)
        {
            Position.x -= (m_selectedItem.itemData.width - 1) * Grid.tileSizeWidth / 2;
            Position.y += (m_selectedItem.itemData.height - 1) * Grid.tileSizeHeight / 2;
            
        }
        return SelectedItemGrid.GetGridPosition(Position);
    }

    #endregion

    /// <summary>
    /// Handle the Highlight Transform position, using the mouse position inside the grid.
    /// </summary>
    private void HandleHighlight()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();
        if (m_oldPositionOnGrid == positionOnGrid)
        {
            return;
        }
        m_oldPositionOnGrid = positionOnGrid;
        if (m_selectedItem == null)
        {
            m_itemToHighlight = SelectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);

            if (m_itemToHighlight != null)
            {
                gridHighlight.Show(true);
                gridHighlight.SetSize(m_itemToHighlight);
                gridHighlight.SetPosition(SelectedItemGrid, m_itemToHighlight);
                UpgradeUI.Description.text = m_itemToHighlight.itemData.itemDescription;
            }
            else {
                gridHighlight.Show(false);
                UpgradeUI.Description.text = "";
            }
           
        }
        else {
            gridHighlight.Show(SelectedItemGrid.BoundryCheck(positionOnGrid.x,positionOnGrid.y,m_selectedItem.itemData.width,m_selectedItem.itemData.height));
            gridHighlight.SetSize(m_selectedItem);
            gridHighlight.SetPosition(SelectedItemGrid, m_selectedItem, positionOnGrid.x, positionOnGrid.y);
        }
    }

    /// <summary>
    /// Create a Random item on the cursor and make it the selected item.
    /// </summary>
    public InventoryItem CreateRandomItem()
    {
        InventoryItem inventoryItem = Instantiate(m_itemPrefab).GetComponent<InventoryItem>();

        m_selectedItem = inventoryItem;

        m_rectTransform = inventoryItem.GetComponent<RectTransform>();
        m_rectTransform.SetParent(m_canvasTransform);
        int selectedItemID = UnityEngine.Random.Range(0, m_itemDataList.Count);
        inventoryItem.Set(m_itemDataList[selectedItemID]);
        return inventoryItem;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_itemData"></param>
    /// <returns></returns>
    public InventoryItem CreateItem(ItemData _itemData)
    {
        InventoryItem inventoryItem = Instantiate(m_itemPrefab).GetComponent<InventoryItem>();
        inventoryItem.Set(_itemData);

        return inventoryItem;
    }

    /// <summary>
    /// Update loop for while an item is being pickedup.
    /// </summary>
    private void PickUpUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2Int tileGridPosition = GetTileGridPosition();


            if (m_selectedItem == null)
            {
                PickUpitem(tileGridPosition);
            }
            else
            {
                DropItem(tileGridPosition);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Vector2Int tileGridPosition = GetTileGridPosition();
            DequipItem(tileGridPosition);
        }
    }

    /// <summary>
    ///  Drop the currently selected item at position on grid.
    /// </summary>
    /// <param name="_posOnGrid"></param>
    private void DropItem(Vector2Int _posOnGrid)
    {
        bool complete = SelectedItemGrid.PlaceItem(m_selectedItem, _posOnGrid.x, _posOnGrid.y, ref m_overlapItem);
       
        if (complete)
        {
            if (SelectedItemGrid.CheckGridType() == Grid.GridType.EQUIPABILITY)
            {
                m_selectedItem.Equip(SelectedItemGrid);
            }
            audioSource.PlayOneShot(dropNoise);
            m_selectedItem = null;
            UpgradeUI.Description.text = "";
            if (m_overlapItem != null)
            {
                m_selectedItem = m_overlapItem;
                m_overlapItem = null;
                m_rectTransform = m_selectedItem.GetComponent<RectTransform>();
            }
            
        }
        
    }

    /// <summary>
    /// Pick up item and attach item grid.
    /// </summary>
    /// <param name="_posOnGrid"></param>
    private void PickUpitem(Vector2Int _posOnGrid)
    {
        m_selectedItem = SelectedItemGrid.PickUpItem(_posOnGrid.x,_posOnGrid.y);
        
        if (m_selectedItem != null)
        {
            audioSource.PlayOneShot(pickupNoise);
            m_rectTransform = m_selectedItem.GetComponent<RectTransform>();
            
            UpgradeUI.Description.text = m_selectedItem.itemData.itemDescription;

            if (SelectedItemGrid.CheckGridType() == Grid.GridType.EQUIPABILITY)
            {
                m_selectedItem.Dequip(SelectedItemGrid);

            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_posOnGrid"></param>
    private void DequipItem(Vector2Int _posOnGrid)
    {
        m_selectedItem = SelectedItemGrid.GetItem(_posOnGrid.x, _posOnGrid.y);

        if (m_selectedItem != null)
        {
            audioSource.PlayOneShot(pickupNoise);

            m_selectedGrid.CleanupItemSlots(m_selectedItem);
            //send it to fuckin nowhere
            m_rectTransform.position = new Vector3(10000, 10000, 10000);
            m_selectedItem.ongridPositionX = -100;
            m_selectedItem.ongridPositionY = -100;
            m_selectedItem.isItemEquppied = false;
            m_selectedItem.gameObject.SetActive(false);
            
            
            m_selectedItem = null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void ItemIconDrag()
    {
        if (m_selectedItem != null)
        {
            m_rectTransform.position = Input.mousePosition;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void Awake()
    {
        if (i == null)
        {
            i = this;
        }
        else
        {
            Debug.Log("LoadoutController Manager fucked up");
            Destroy(gameObject);
        }
        gridHighlight = GetComponent<GridHighlight>();
        UpgradeUI = FindObjectOfType<Scr_UpgradeUI>();
        FindObjectOfType<script_WeaponSwap>().Start();
        UpgradeUI.gameObject.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isGridActive)
            {
                SetUIActive(true);
            }
            else {
                SetUIActive(false);
            }
        }
        if (isGridActive)
        {
            ItemIconDrag();
            if (SelectedItemGrid == null)
            {
                gridHighlight.Show(false);
                return;
            }

            HandleHighlight();

            //Interaction Update for inventory
            PickUpUpdate();
        }
       

    }
}
