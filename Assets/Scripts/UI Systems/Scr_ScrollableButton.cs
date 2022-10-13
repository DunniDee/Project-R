using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Scr_ScrollableButton : MonoBehaviour, IPointerClickHandler
{
    [Header("UI Elements")]
    public Image GunImage;

    [SerializeField] private TMP_Text GunNameText;
    [SerializeField] private TMP_Text GunDamageText;
    [SerializeField] private TMP_Text GunMaxAmmo;

    [SerializeField] int weaponToEquipIndex = 0;
    public void SetEquipIndex(int _index)
    {
        weaponToEquipIndex = _index;
    }

    public int GetEquipIndex() { return weaponToEquipIndex; }

    public void OnPointerClick(PointerEventData eventData)
    {
        Scr_LoadOutUI.i.SetSelectedButton(this);
        Debug.Log("Pressed Button");
    }

    public void SetMaxAmmoUI(float _MaxAmmo)
    {
        GunMaxAmmo.text = _MaxAmmo.ToString() + "+" + _MaxAmmo.ToString();
    }
    public void SetGunNameUI(string _GunName)
    {
        GunNameText.text = _GunName;
    }

    public void SetUIElements(string _gunName, string _gunDamage, string _gunMaxAmmo, Sprite _gunImage)
    {
        GunNameText.text = _gunName;
        GunDamageText.text = _gunDamage;
        GunMaxAmmo.text = _gunMaxAmmo + "/" + _gunMaxAmmo;
        GunImage.sprite = _gunImage;
    }

    // Start is called before the first frame update
  

}
