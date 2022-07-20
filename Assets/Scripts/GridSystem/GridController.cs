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

    public enum GridType { 
        Static,
        StaticDragAble,
        EffectEnabled,
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

    public void Awake()
    {
        gridHighlight = GetComponent<GridHighlight>();
    }

    void Update()
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
}
