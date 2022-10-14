using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Scr_UIButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Scr_MenuController.i.PlayClickAudio();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Scr_MenuController.i.PlayRollOverAudio();
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
