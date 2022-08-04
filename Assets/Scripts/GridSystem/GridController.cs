using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{
    [HideInInspector]
    private Grid selectedGrid;
    public Grid SelectedItemGrid { 
        get => selectedGrid;
        set {
            selectedGrid = value;
            gridHighlight.SetParent(value);
        }
    }



    InventoryItem selectedItem;
    InventoryItem overlapItem;
    RectTransform rectTransform;

    [SerializeField] List<ItemData> items;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform canvasTransform;

    [SerializeField] Slider slider;
    public Transform GetUICanvas()
    {
        return canvasTransform;
    }
    GridHighlight gridHighlight;
    [SerializeField]Scr_UpgradeUI UpgradeUI;

    public AudioSource audioSource;
    public AudioClip pickupNoise;
    public AudioClip dropNoise;

    bool isGridActive = false;

    public bool GetIsGridActive() { return isGridActive; }
    public void SetIsGridActive(bool _b) { isGridActive = _b; }

    public void SetFullScreen(bool _Fullscreen)
    {
        Screen.fullScreen = _Fullscreen;
    }
    public void SetQuality(int _graphicsIndex)
    {
        QualitySettings.SetQualityLevel(_graphicsIndex);
    }

    public void SetMasterAudio(float _Audio)
    {
        AudioListener.volume = _Audio;
    }
    public void Awake()
    {
        gridHighlight = GetComponent<GridHighlight>();
        UpgradeUI = FindObjectOfType<Scr_UpgradeUI>();
        FindObjectOfType<script_WeaponSwap>().Start();
        UpgradeUI.SetWeaponUIElements();
        UpgradeUI.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isGridActive)
            {
                ActivateGrid(true);
            }
            else {
                ActivateGrid(false);
            }
        }
        if (isGridActive)
        {
            ItemIconDrag();
            if (Input.GetKeyDown(KeyCode.Q))
            {
                CreateRandomItem();
            }
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

    public void ActivateGrid(bool _b)
    {
        FindObjectOfType<Scr_PlayerLook>().enabled = !_b;
        SetIsGridActive(_b);
        canvasTransform.gameObject.SetActive(_b);
        UpgradeUI.enabled = _b;
        Cursor.visible = _b;
        Cursor.lockState = Cursor.visible ? CursorLockMode.Confined : CursorLockMode.Locked;
        
    }

    Vector2Int oldPositionOnGrid;
    InventoryItem itemToHighlight;
    private void HandleHighlight()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();
        if (oldPositionOnGrid == positionOnGrid)
        {
            return;
        }
        oldPositionOnGrid = positionOnGrid;
        if (selectedItem == null)
        {
            itemToHighlight = SelectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);

            if (itemToHighlight != null)
            {
                gridHighlight.Show(true);
                gridHighlight.SetSize(itemToHighlight);
                gridHighlight.SetPosition(SelectedItemGrid, itemToHighlight);
                UpgradeUI.Description.text = itemToHighlight.itemData.itemDescription;
            }
            else {
                gridHighlight.Show(false);
                UpgradeUI.Description.text = "";
            }
           
        }
        else {
            gridHighlight.Show(SelectedItemGrid.BoundryCheck(positionOnGrid.x,positionOnGrid.y,selectedItem.itemData.width,selectedItem.itemData.height));
            gridHighlight.SetSize(selectedItem);
            gridHighlight.SetPosition(SelectedItemGrid, selectedItem, positionOnGrid.x, positionOnGrid.y);
        }
    }

    private void CreateRandomItem()
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();

        selectedItem = inventoryItem;

        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);
        int selectedItemID = UnityEngine.Random.Range(0, items.Count);
        inventoryItem.Set(items[selectedItemID]);
    }

    private void PickUpUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2Int tileGridPosition = GetTileGridPosition();

            
            if (selectedItem == null)
            {
                PickUpitem(tileGridPosition);
            }
            else
            {
                DropItem(tileGridPosition);
            }
        }
    }

    public Vector3 GetMouseWorldPosition(Camera camera, float worldDepth)
    {
        Vector2 screenPosition = Input.mousePosition;
        Vector3 screenPositionWithDepth = new Vector3(screenPosition.x, screenPosition.y, worldDepth);
        Debug.Log(camera.ScreenToWorldPoint(screenPositionWithDepth));
        return camera.ScreenToWorldPoint(screenPositionWithDepth);
    }
    public Vector2Int GetTileGridPosition()
    {
        Vector2 Position = Input.mousePosition;
        if (selectedItem != null)
        {
            Position.x -= (selectedItem.itemData.width - 1) * Grid.tileSizeWidth / 2;
            Position.y += (selectedItem.itemData.height - 1) * Grid.tileSizeHeight / 2;
            
        }
        return SelectedItemGrid.GetGridPosition(Position);
    }
    private void DropItem(Vector2Int posOnGrid)
    {
        bool complete = SelectedItemGrid.PlaceItem(selectedItem, posOnGrid.x, posOnGrid.y, ref overlapItem);
       
        if (complete)
        {
            if (SelectedItemGrid.CheckGridType() == Grid.GridType.EQUIPABILITY)
            {
                selectedItem.Equip(SelectedItemGrid);
                UpgradeUI.SetWeaponUIElements();
            }
            audioSource.PlayOneShot(dropNoise);
            selectedItem = null;
            UpgradeUI.Description.text = "";
            if (overlapItem != null)
            {
                selectedItem = overlapItem;
                overlapItem = null;
                rectTransform = selectedItem.GetComponent<RectTransform>();
            }
            
        }
        
    }

    private void PickUpitem(Vector2Int posOnGrid)
    {
        selectedItem = SelectedItemGrid.PickUpItem(posOnGrid.x, posOnGrid.y);
        
        if (selectedItem != null)
        {
            audioSource.PlayOneShot(pickupNoise);
            rectTransform = selectedItem.GetComponent<RectTransform>();
            
            UpgradeUI.Description.text = selectedItem.itemData.itemDescription;

            if (SelectedItemGrid.CheckGridType() == Grid.GridType.EQUIPABILITY)
            {
                selectedItem.Dequip(SelectedItemGrid);
                UpgradeUI.SetWeaponUIElements();

            }
        }

    }

    private void ItemIconDrag()
    {
        if (selectedItem != null)
        {
            rectTransform.position = Input.mousePosition;
        }
    }

    public ItemData GetSelectedItemData()
    {
        return selectedItem.itemData;
    }
}
