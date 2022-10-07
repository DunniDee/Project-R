using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class scr_EquippedWeaponButton : MonoBehaviour,  IPointerClickHandler
{
    [Header("UI Elements")]
    public Image GunImage;

    [SerializeField] private TMP_Text GunNameText;
    [SerializeField] private TMP_Text GunDamageText;
    [SerializeField] private TMP_Text GunMaxAmmo;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Scr_LoadOutUI.i.GetSelectedButton())
        {
            Scr_LoadOutUI.i.EquipWeapon();
        }
    }

    public void SetUIElements(string _gunName, string _gunDamage, string _gunMaxAmmo, Sprite _gunImage)
    {
        GunNameText.text = _gunName;
        GunDamageText.text = _gunDamage;
        GunMaxAmmo.text = _gunMaxAmmo + "/" + _gunMaxAmmo;
        GunImage.sprite = _gunImage;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
