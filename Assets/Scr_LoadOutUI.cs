using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scr_LoadOutUI : MonoBehaviour
{

    public List<TMP_Text> WeaponSlots;
    private Scr_ScrollableButton[] AvaliableWeapons;

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
        var selectedGun = script_WeaponSwap.Instance.Weapons[selectedButton.weaponToEquipIndex].GetComponent<Script_WeaponBase>();
        UI_GunDescription.title.text = selectedGun.GetGunName();
        UI_GunDescription.Damage.text = selectedGun.GetDamage().ToString();
        UI_GunDescription.Firerate.text = selectedGun.GetFireRate().ToString();
        UI_GunDescription.MagazineSize.text = selectedGun.GetMagCount().ToString();
        UI_GunDescription.FiringType.text = "SemiAuto (Add FiringType to Weapon)";

    }
    public void EquipWeapon()
    {
        //Dont equip weapon that is already equipped
        if (selectedButton.weaponToEquipIndex == weaponToSwapIndex) return;
        var selectedGun = script_WeaponSwap.Instance.Weapons[selectedButton.weaponToEquipIndex].GetComponent<Script_WeaponBase>();

        script_WeaponSwap.Instance.EquippedWeapons[weaponToSwapIndex].SetActive(false);
        script_WeaponSwap.Instance.EquippedWeapons[weaponToSwapIndex] = script_WeaponSwap.Instance.Weapons[selectedButton.weaponToEquipIndex];

        WeaponSlots[weaponToSwapIndex].text = selectedGun.GetGunName();
    }

    public void SetWeaponSlots()
    {
        int i = 0;
        foreach(TMP_Text text in WeaponSlots)
        {
            var weapon = script_WeaponSwap.Instance.EquippedWeapons[i].GetComponent<Script_WeaponBase>();
            text.text = weapon.GetGunName();
            i++;
        }
    }

    public void SetAvaliableWeapons()
    {
        for(int i = 0; i < script_WeaponSwap.Instance.Weapons.Length; i++)
        {
            AvaliableWeapons[i].tmpText.text = script_WeaponSwap.Instance.Weapons[i].GetComponent<Script_WeaponBase>().GetGunName();
            AvaliableWeapons[i].weaponToEquipIndex = i;
        }
    }

   
    // Start is called before the first frame update
    void Start()
    {
        SetWeaponSlots();
        AvaliableWeapons = GetComponentsInChildren<Scr_ScrollableButton>();
        SetAvaliableWeapons();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
