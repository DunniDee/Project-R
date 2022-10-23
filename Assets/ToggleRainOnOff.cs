using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleRainOnOff : MonoBehaviour
{
    public bool toggleBool = true;
    public GameObject Rain;
    void OnTriggerExit(Collider target)
    {
        if (target.tag == "Player")
        {
            toggleBool = !toggleBool;
            Rain.SetActive(!toggleBool);
        }
    }
}
