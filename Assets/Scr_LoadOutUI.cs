using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scr_LoadOutUI : MonoBehaviour
{
    public List<TMP_Text> WeaponSlots;

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

    // Start is called before the first frame update
    void Start()
    {
        SetWeaponSlots();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
