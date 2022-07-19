using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_CameraEffects : MonoBehaviour
{ 
    [SerializeField] Camera Cam;
    [SerializeField] float RotateLerpDecay;
    [SerializeField] float FOVLerpDecay;

    
    public float FovTo;
    public Vector3 RotateTo;
    public Vector3 LerpPos;



    // Update is called once per frame
    void Update()
    {
        RotateTo = Vector3.Lerp(RotateTo, Vector3.zero, Time.deltaTime * RotateLerpDecay);
        transform.localRotation = Quaternion.Lerp(transform.localRotation,Quaternion.Euler(RotateTo), Time.deltaTime * 5);

        transform.localPosition = Vector3.Lerp(transform.localPosition, LerpPos, Time.deltaTime * 5);
        
        FovTo = Mathf.Lerp(FovTo, 70, Time.deltaTime * FOVLerpDecay);
        Cam.fieldOfView = Mathf.Lerp(Cam.fieldOfView, FovTo, Time.deltaTime * FOVLerpDecay);
    }
}
