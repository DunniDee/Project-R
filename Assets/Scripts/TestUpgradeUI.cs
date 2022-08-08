using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestUpgradeUI : MonoBehaviour
{
    [SerializeField] GridController gridController;
    [SerializeField] TMP_Text TMP_Description;
    float Health = 100;

    // Start is called before the first frame update
    void Start()
    {
        gridController = FindObjectOfType<GridController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gridController.GetSelectedItemData() != null && TMP_Description.text != gridController.GetSelectedItemData().itemDescription)
        {
            TMP_Description.text = gridController.GetSelectedItemData().itemDescription;
        }
    }
}
