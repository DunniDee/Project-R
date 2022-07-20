using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(scr_AbilityGrid))]
public class Scr_gridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    AbilityController abilityController;
    scr_AbilityGrid abilityGrid;

    public void OnPointerEnter(PointerEventData _eventData)
    {
        Debug.Log("Pointer Enter");
        abilityController.SelectedItemGrid = abilityGrid;
    }

    public void OnPointerExit(PointerEventData _eventData)
    {
        Debug.Log("Pointer Exit");
        abilityController.SelectedItemGrid = null;
    }


    // Start is called before the first frame update
    void Start()
    {
        abilityGrid = GetComponent<scr_AbilityGrid>();
        abilityController = FindObjectOfType(typeof(AbilityController)) as AbilityController;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
