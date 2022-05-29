using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMapOnOff : MonoBehaviour
{
    public bool OnOff = false;
    public GameObject MapModel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(OnOff == true)
        {
            MapModel.SetActive(true);

        }
        else
        {
            MapModel.SetActive(false);

        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnOff = !OnOff;
        }

    }
}
