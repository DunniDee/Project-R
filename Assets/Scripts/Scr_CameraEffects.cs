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
    public float Fov;
    public Quaternion Rotation;
    public Vector3 RotateTo;
    public Vector3 LerpPos;
    public Vector3 LerpTo;




    // Update is called once per frame
    void Update()
    {   
        //rotations are lerped to the resting position
        RotateTo = Vector3.Lerp(RotateTo, Vector3.zero, Time.deltaTime * RotateLerpDecay);
        Rotation = Quaternion.Lerp(transform.localRotation,Quaternion.Euler(RotateTo), Time.deltaTime * 5);
        transform.localRotation = Rotation;

        //ShakeAmplitude are lerped to the resting position
        ShakeAmplitude = Mathf.Lerp(ShakeAmplitude, 0, Time.deltaTime * 5);
        ShakeTimer += Time.deltaTime;
        ShakeRotation = ShakeAmplitude * Mathf.Sin(ShakeTimer * 100);
        ShakeTransform.localRotation = Quaternion.Euler(0,0,ShakeRotation);

        //Lerp to is the postion
        LerpTo =  Vector3.Lerp(LerpTo, LerpPos, Time.deltaTime * 20);
        transform.localPosition = Vector3.Lerp(transform.localPosition, LerpTo, Time.deltaTime * 20);
        
        //ShakeAmplitude are lerped to the resting position
        FovTo = Mathf.Lerp(FovTo, 70, Time.deltaTime * FOVLerpDecay);
        Fov = Mathf.Lerp(Fov, FovTo, Time.deltaTime * FOVLerpDecay);
        Cam.fieldOfView = Fov;
    }
}
