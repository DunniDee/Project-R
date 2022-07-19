using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_CameraEffects : MonoBehaviour
{ 
    [SerializeField] float LerpAcceleration;
    public Vector3 RotateTo;

    // Update is called once per frame
    void Update()
    {
        RotateTo = Vector3.Lerp(RotateTo, Vector3.zero, Time.deltaTime * LerpAcceleration);

        transform.localRotation = Quaternion.Lerp(transform.localRotation,Quaternion.Euler(RotateTo), Time.deltaTime * 5);
    }
}
