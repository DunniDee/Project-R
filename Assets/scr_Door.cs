using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Door : MonoBehaviour
{
[SerializeField] GameObject doorL;
[SerializeField] GameObject doorR;

[SerializeField] bool isInRange;
[SerializeField] bool CanBeOpened = true;




    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
        }
    }


    private void Update()
    {
        if (isInRange && CanBeOpened)
        {
            doorL.transform.localPosition = Vector3.Lerp(doorL.transform.localPosition, Vector3.forward,Time.deltaTime * 3);
            doorR.transform.localPosition = Vector3.Lerp(doorR.transform.localPosition, Vector3.back,Time.deltaTime * 3);
        }
        else
        {
            doorL.transform.localPosition = Vector3.Lerp(doorL.transform.localPosition, Vector3.zero,Time.deltaTime * 3);
            doorR.transform.localPosition = Vector3.Lerp(doorR.transform.localPosition, Vector3.zero,Time.deltaTime * 3);
        }
    }
}
