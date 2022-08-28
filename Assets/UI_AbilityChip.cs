using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_AbilityChip : MonoBehaviour
{
    Button button;
    [SerializeField] Image isEquippedImage;

    [SerializeField] InventoryItem attachedItem;

    public void SetItemData(ItemData _itemdata)
    {
        attachedItem.Set(_itemdata);
    }

    public InventoryItem GetItem()
    {
        return attachedItem;
    }

    void Init()
    {
        int i = Random.Range(0, LoadoutController.i.m_itemDataList.Count);
        attachedItem = LoadoutController.i.CreateItem(LoadoutController.i.m_itemDataList[0]);
    }

    public void PickUp()
    {
        if (LoadoutController.i.m_selectedItem) return;

        if (attachedItem == null || !attachedItem.isItemEquppied)
        {
            attachedItem.gameObject.SetActive(true);
            LoadoutController.i.m_selectedItem = attachedItem;
            LoadoutController.i.m_rectTransform = attachedItem.GetComponent<RectTransform>();
            LoadoutController.i.m_rectTransform.SetParent(LoadoutController.i.m_canvasTransform);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        //isEquippedImage = GetComponent<Image>();
        button.onClick.AddListener(PickUp);
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (attachedItem.isItemEquppied)
        {
            isEquippedImage.color = Color.green;
        }
        else
        {
            isEquippedImage.color = Color.red;
        }
    }
}
