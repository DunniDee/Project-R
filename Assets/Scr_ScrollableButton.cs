using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class Scr_ScrollableButton : MonoBehaviour, IPointerClickHandler
{
    private Scr_LoadOutUI loadoutUI;
    public TMP_Text tmpText;
    public int weaponToEquipIndex = 0;

    public void OnPointerClick(PointerEventData eventData)
    {
        loadoutUI = GetComponentInParent<Scr_LoadOutUI>();
        loadoutUI.SetSelectedButton(this);
        loadoutUI.SetSelectedGunUI();
    }

    // Start is called before the first frame update
    void Awake()
    {
        tmpText = GetComponentInChildren<TMP_Text>();
    }

}
