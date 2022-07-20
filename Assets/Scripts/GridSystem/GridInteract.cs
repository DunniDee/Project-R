using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Grid))]
public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    GridController gridController;
    Grid grid;

    public void OnPointerEnter(PointerEventData _eventData)
    {
        Debug.Log("Pointer Enter");
        gridController.SelectedItemGrid = grid;
    }

    public void OnPointerExit(PointerEventData _eventData)
    {
        Debug.Log("Pointer Exit");
        gridController.SelectedItemGrid = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponent<Grid>();
        gridController = FindObjectOfType(typeof(GridController)) as GridController;
    }

}
