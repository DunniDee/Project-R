using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_CameraTilt : MonoBehaviour
{ 
    [SerializeField] float RotateLerpDecay;
    [SerializeField] float FOVLerpDecay;
    public Vector3 RotateTo;



    // Update is called once per frame
    void Update()
    {
        RotateTo = Vector3.Lerp(RotateTo, Vector3.zero, Time.deltaTime * RotateLerpDecay);
        transform.localRotation = Quaternion.Lerp(transform.localRotation,Quaternion.Euler(RotateTo), Time.deltaTime * 5);
    }
}
