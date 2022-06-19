using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMapOnOff : MonoBehaviour
{
    public bool OnOnly = false;
    public bool OffOnly = false;
    public bool Model = false;
    public GameObject MapModel;

    // Start is called before the first frame update
    void Start()
    {
        MapModel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        

     /*   if (Model == true)
        {
            MapModel.SetActive(true);

        }
        if (Model == false)
        {
            MapModel.SetActive(false);

        }*/

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && OnOnly)
        {
            MapModel.SetActive(true);
        }

        

    }

    void OnTriggerExit(Collider other)
    { 
    if (other.CompareTag("Player") && OffOnly)
        {
            MapModel.SetActive(false);
        }
    }


}
