using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_CultistCache : MonoBehaviour
{
    public GameObject CultistCanvas;


    [SerializeField] List<InventoryItem> itemList;

    [SerializeField] int selectedIndex;

    public void SetSelectedIndex(int _i)
    {
        selectedIndex = _i;
    }

    public void EnableCanvas(bool _b)
    {
        FindObjectOfType<Scr_PlayerMotor>().enabled = !_b;
        FindObjectOfType<Scr_PlayerLook>().enabled = !_b;
        FindObjectOfType<Scr_PlayerHealth>().enabled = !_b;
        script_WeaponSwap.Instance.SetCanShoot(!_b);

        CultistCanvas.SetActive(_b);
        Cursor.visible = _b;
        Cursor.lockState = _b ? CursorLockMode.Confined : CursorLockMode.Locked;
    }


    public void ConfirmSelection()
    {
        Scr_UpgradeUI.i.AddAbilityChip(itemList[selectedIndex]);
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            itemList.Add(LoadoutController.i.CreateItem(LoadoutController.i.m_itemDataList[Random.Range(0, LoadoutController.i.m_itemDataList.Count)]));
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
