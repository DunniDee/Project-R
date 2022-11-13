using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_titledrop : MonoBehaviour
{
    public GameObject titlecard;

    void Start()
    {
        titlecard.SetActive(false);
    }

    void OnTriggerEnter()
    {
        titlecard.SetActive(true);
    }
}
