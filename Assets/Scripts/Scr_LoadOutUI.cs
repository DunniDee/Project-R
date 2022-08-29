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
    }

    public List<TMP_Text> UI_EquippedWeaponSlots;
    private Scr_ScrollableButton[] UI_AvaliableWeapons;

    Scr_ScrollableButton selectedButton;

    [SerializeField] Image dragImage;

    public int weaponToSwapIndex = 0;

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
    public void EquipWeapon()
    {
        //Dont equip weapon that is already equipped
        if (selectedButton.GetEquipIndex() == weaponToSwapIndex) return;

        for (int i = 0; i < script_WeaponSwap.Instance.EquippedWeapons.Count; i++)
        {
          
        }

        var selectedGun = script_WeaponSwap.Instance.Weapons[selectedButton.GetEquipIndex()].GetComponent<Script_WeaponBase>();

        //Set the currently active weapon to false as its no longer equipped
        script_WeaponSwap.Instance.EquippedWeapons[weaponToSwapIndex].SetActive(false);

        script_WeaponSwap.Instance.EquippedWeapons[weaponToSwapIndex] = script_WeaponSwap.Instance.Weapons[selectedButton.GetEquipIndex()];

        script_WeaponSwap.Instance.EquippedWeapons[weaponToSwapIndex].SetActive(true);

        //Change name of Weapon slot to equipped item.
        UI_EquippedWeaponSlots[weaponToSwapIndex].text = selectedGun.GetGunName();
    }

    public void SetEquippedWeaponSlots()
    {
        int i = 0;
        //Set the name of the Equipped Weapon Slots UI
        foreach(TMP_Text text in UI_EquippedWeaponSlots)
        {
            var weapon = script_WeaponSwap.Instance.EquippedWeapons[i].GetComponent<Script_WeaponBase>();
            text.text = weapon.GetGunName();
            i++;
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
        UI_AvaliableWeapons = GetComponentsInChildren<Scr_ScrollableButton>();
        SetEquippedWeaponSlots();
        SetAvaliableWeapons();
    }

    // Update is called once per frame
    void Update()
    {
        HandleIconDrag();
    }
}
