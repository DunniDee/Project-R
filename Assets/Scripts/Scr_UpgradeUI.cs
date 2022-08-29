using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scr_UpgradeUI : MonoBehaviour
{
    public TMP_Text Description;

    public static Scr_UpgradeUI i;

    public void Awake()
    {
        if (i == null)
        {
            i = this;
        }
        else
        {
            Debug.Log("Damagepopup Manager fucked up");
            Destroy(gameObject);
        }
    }

    public GameObject AbilityChipContainer;
    [SerializeField] GameObject AbilityChipUI;

    public List<GameObject> MenuTabs;
    public int currentIndex = 0;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_itemdata"></param>
    public void AddAbilityChip(InventoryItem _itemdata)
    {
        var newAbilityChip = Instantiate(AbilityChipUI, AbilityChipContainer.transform);
        UI_AbilityChip Abilitydata = newAbilityChip.GetComponent<UI_AbilityChip>();

        Abilitydata.SetItemData(_itemdata);
    }


    public void SetWeaponUIElements()
    {

    }

    public void SetCurrentIndex(int _Index)
    {
        currentIndex = _Index;
    }

    public void Update()
    {
        if (!MenuTabs[currentIndex].activeSelf)
        {
            MenuTabs[currentIndex].SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddAbilityChip(LoadoutController.i.CreateItem(LoadoutController.i.m_itemDataList[Random.Range(0, LoadoutController.i.m_itemDataList.Count)]));
        }
        foreach (GameObject go in MenuTabs)
        {
            if (go != MenuTabs[currentIndex] && go.activeSelf)
            {
                go.SetActive(false);
            }
        }
        
    }
}
