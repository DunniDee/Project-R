using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour
{
    [HideInInspector]
    private scr_AbilityGrid selectedItemGrid;
    public scr_AbilityGrid SelectedItemGrid { 
        get => selectedItemGrid;
        set {
            selectedItemGrid = value;
            inventoryHighlight.SetParent(value);
        }
    }

    public enum GridType { 
        Static,
        StaticDragAble,
        EffectEnabled,
    }

    scr_InventoryItem selectedItem;
    scr_InventoryItem overlapItem;
    RectTransform rectTransform;

    [SerializeField] List<ItemData> items;
    [SerializeField] GameObject Prefab;
    [SerializeField] Transform canvasTransform;

    InventoryHighlight inventoryHighlight;

    public AudioSource audioSource;
    public AudioClip pickupNoise;
    public AudioClip dropNoise;

    public void Awake()
    {
        inventoryHighlight = GetComponent<InventoryHighlight>();
    }



    // Update is called once per frame
    void Update()
    {
        ItemIconDrag();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreateRandomItem();
        }
        if (SelectedItemGrid == null)
        {
            inventoryHighlight.Show(false);
            return;
        }

        HandleHighlight();

        //Interaction Update for inventory
        PickUpUpdate();

    }

    //Handle TileHighlight
    Vector2Int oldPositionOnGrid;
    scr_InventoryItem itemToHighlight;
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
                inventoryHighlight.Show(true);
                inventoryHighlight.SetSize(itemToHighlight);
                inventoryHighlight.SetPosition(SelectedItemGrid, itemToHighlight);
            }
            else {
                inventoryHighlight.Show(false);
            }
           
        }
        else {
            inventoryHighlight.Show(SelectedItemGrid.BoundryCheck(positionOnGrid.x,positionOnGrid.y,selectedItem.itemData.width,selectedItem.itemData.height));
            inventoryHighlight.SetSize(selectedItem);
            inventoryHighlight.SetPosition(SelectedItemGrid, selectedItem, positionOnGrid.x, positionOnGrid.y);
        }
    }

    private void CreateRandomItem()
    {
        scr_InventoryItem inventoryItem = Instantiate(Prefab).GetComponent<scr_InventoryItem>();

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
                Pickupitem(tileGridPosition);
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
            Position.x -= (selectedItem.itemData.width - 1) * scr_AbilityGrid.tileSizeWidth / 2;
            Position.y += (selectedItem.itemData.height - 1) * scr_AbilityGrid.tileSizeHeight / 2;
            
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

    private void Pickupitem(Vector2Int posOnGrid)
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
