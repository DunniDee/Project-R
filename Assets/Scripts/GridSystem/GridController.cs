using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    GridHighlight gridHighlight;

    public AudioSource audioSource;
    public AudioClip pickupNoise;
    public AudioClip dropNoise;

    bool isGridActive = false;

    public bool GetIsGridActive() { return isGridActive; }
    public void SetIsGridActive(bool _b) { isGridActive = _b; }

    public void Awake()
    {
        gridHighlight = GetComponent<GridHighlight>();
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

    private void ActivateGrid(bool _b)
    {
        SetIsGridActive(_b);
        canvasTransform.gameObject.SetActive(_b);
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
            }
            else {
                gridHighlight.Show(false);
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
            audioSource.PlayOneShot(dropNoise);
            selectedItem = null;
            if (overlapItem != null)
            {
                selectedItem = overlapItem;
                overlapItem = null;
                rectTransform = selectedItem.GetComponent<RectTransform>();
            }
            SelectedItemGrid.CheckGridType();
        }
        
    }

    private void PickUpitem(Vector2Int posOnGrid)
    {
        selectedItem = SelectedItemGrid.PickUpItem(posOnGrid.x, posOnGrid.y);
        
        if (selectedItem != null)
        {
            audioSource.PlayOneShot(pickupNoise);
            rectTransform = selectedItem.GetComponent<RectTransform>();
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
