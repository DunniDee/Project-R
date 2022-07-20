using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_CameraEffects : MonoBehaviour
{ 
    [SerializeField] Camera Cam;
    [SerializeField] Transform ShakeTransform;
    [SerializeField] float RotateLerpDecay;
    [SerializeField] float FOVLerpDecay;

    float ShakeRotation;
    float ShakeTimer;

    public float ShakeAmplitude;
    public float ShakeTime;
    public float FovTo;
    public Vector3 RotateTo;
    public Vector3 LerpPos;



    // Update is called once per frame
    void Update()
    {
        RotateTo = Vector3.Lerp(RotateTo, Vector3.zero, Time.deltaTime * RotateLerpDecay);
        transform.localRotation = Quaternion.Lerp(transform.localRotation,Quaternion.Euler(RotateTo), Time.deltaTime * 5);


        ShakeAmplitude = Mathf.Lerp(ShakeAmplitude, 0, Time.deltaTime * 5);
        ShakeTimer += Time.deltaTime;
        ShakeRotation = ShakeAmplitude * Mathf.Sin(ShakeTimer * 100);
        ShakeTransform.localRotation = Quaternion.Euler(0,0,ShakeRotation);

        transform.localPosition = Vector3.Lerp(transform.localPosition, LerpPos, Time.deltaTime * 5);
        
        FovTo = Mathf.Lerp(FovTo, 70, Time.deltaTime * FOVLerpDecay);
        Cam.fieldOfView = Mathf.Lerp(Cam.fieldOfView, FovTo, Time.deltaTime * FOVLerpDecay);
    }
}
