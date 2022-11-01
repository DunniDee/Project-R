using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scr_LoadOutUI : MonoBehaviour
{
    public static Scr_LoadOutUI i;

    public void Awake()
    {
        if (i == null)
        {
            i = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //Initalise Avaliable & Equipped Weapon Arrays
        UI_AvaliableWeapons = GetComponentsInChildren<Scr_ScrollableButton>();
        UI_EquippedWeapons = GetComponentsInChildren<scr_EquippedWeaponButton>();
    }

    private Scr_ScrollableButton[] UI_AvaliableWeapons;
    [SerializeField] scr_EquippedWeaponButton[] UI_EquippedWeapons;


    Scr_ScrollableButton selectedButton;

    [SerializeField] Image dragImage;

    public int weaponToSwapIndex = 0;

    public Scr_ScrollableButton GetSelectedButton()
    {
        if (selectedButton != null)
        {
            return selectedButton;
        }
        else
        {
            Debug.LogError("SelectedButton Does not exist");
            return null;
        }
    }

    public void SetSelectedButton(Scr_ScrollableButton _button)
    {
        selectedButton = null;
        selectedButton = _button;
    }

    public void SetWeaponToSwap(int _index)
    {
        weaponToSwapIndex = _index;
    }

    public void HandleIconDrag()
    { 
        if(selectedButton != null)
        {
            dragImage.sprite = selectedButton.GunImage.sprite;
            dragImage.rectTransform.position = Input.mousePosition;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool CheckIfWeaponEquipped()
    {
        int i = 0;
        foreach (scr_EquippedWeaponButton equippedWeapon in UI_EquippedWeapons)
        {
            if (equippedWeapon == UI_AvaliableWeapons[selectedButton.GetEquipIndex()])
            {
                return false;
            }
            i++;
        }
        return true;
    }
    /// <summary>
    /// 
    /// </summary>
    public void EquipWeapon()
    {
        script_WeaponSwap.Instance.EquippedWeapons[weaponToSwapIndex] = script_WeaponSwap.Instance.Weapons[selectedButton.GetEquipIndex()];

        script_WeaponSwap.Instance.EquippedWeapons[weaponToSwapIndex].SetActive(true);

        //Change name of Weapon slot to equipped item.
        SetEquippedWeaponSlots();

        selectedButton = null;
        dragImage.rectTransform.position = Vector3.one * 10000f;


    }

    public void SetEquippedWeaponSlots()
    {
        //Set the name of the Equipped Weapon Slots UI
        for (int i = 0; i < script_WeaponSwap.Instance.EquippedWeapons.Count; i++)
        {
            Script_WeaponBase gun = script_WeaponSwap.Instance.EquippedWeapons[i].GetComponent<Script_WeaponBase>();

            UI_EquippedWeapons[i].SetUIElements(gun.GetGunName(), gun.GetDamage().ToString(), gun.GetMagCount().ToString(), gun.GetGunSprite());
        }
    }
   
    public void SetAvaliableWeapons()
    {
        //Get avaliable Weapons from weapon swap and load the buttons with their index's
        for(int i = 0; i < script_WeaponSwap.Instance.Weapons.Length; i++)
        {
            Script_WeaponBase gun = script_WeaponSwap.Instance.Weapons[i].GetComponent<Script_WeaponBase>();


            UI_AvaliableWeapons[i].SetUIElements(gun.GetGunName(), gun.GetDamage().ToString(), gun.GetMagCount().ToString(), gun.GetGunSprite());
            UI_AvaliableWeapons[i].SetEquipIndex(i);
        }
    }

   
    // Start is called before the first frame update
    void Start()
    {
        InitaliseLoadout();
    }

    public void InitaliseLoadout()
    {
        //Set UI Elemements
        SetEquippedWeaponSlots();
        SetAvaliableWeapons();
    }

    // Update is called once per frame
    void Update()
    {
        HandleIconDrag();
    }
}
