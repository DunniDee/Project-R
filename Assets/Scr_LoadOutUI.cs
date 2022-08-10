using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scr_LoadOutUI : MonoBehaviour
{

    public List<TMP_Text> UI_EquippedWeaponSlots;
    private Scr_ScrollableButton[] UI_AvaliableWeapons;

    [System.Serializable]
    public struct GunDescription {
        public TMP_Text title;
        public TMP_Text Damage;
        public TMP_Text Firerate;
        public TMP_Text MagazineSize;
        public TMP_Text FiringType;
    }
    public GunDescription UI_GunDescription;

    Scr_ScrollableButton selectedButton;

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

    public void SetSelectedGunUI()
    {
        var selectedGun = script_WeaponSwap.Instance.Weapons[selectedButton.GetEquipIndex()].GetComponent<Script_WeaponBase>();
        UI_GunDescription.title.text = selectedGun.GetGunName();
        UI_GunDescription.Damage.text = selectedGun.GetDamage().ToString();
        UI_GunDescription.Firerate.text = selectedGun.GetFireRate().ToString();
        UI_GunDescription.MagazineSize.text = selectedGun.GetMagCount().ToString();
        UI_GunDescription.FiringType.text = "SemiAuto (Add FiringType to Weapon)";

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

    public void SetWeaponSlots()
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


            UI_AvaliableWeapons[i].SetUIElements(gun.GetGunName(), gun.GetDamage().ToString(), gun.GetMagCount().ToString());
            UI_AvaliableWeapons[i].SetEquipIndex(i);
        }
    }

   
    // Start is called before the first frame update
    void Start()
    {
        UI_AvaliableWeapons = GetComponentsInChildren<Scr_ScrollableButton>();
        SetWeaponSlots();
        SetAvaliableWeapons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
