using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Script_PlayerLook : MonoBehaviour
{
    [Header("Mouse Sensitivity")]
    [Range(0,500)]
    [SerializeField] private float SenseX;
    [Range(0,500)]
    [SerializeField] private float SenseY;

    [Header("Camera")]
    [SerializeField] private Transform CameraHolder;
    [SerializeField] private Transform CameraRotator;
    [SerializeField] private Transform Gun;
    bool hasLanded;
    [SerializeField] private Transform CameraRecoiler;
    [SerializeField] private Transform CameraShaker;
    [SerializeField] private Camera MainCam;
    [SerializeField] private Camera FPCam;
    [Space]

    [Header("Orientation")]
    [SerializeField] private Transform Orientation;

    [SerializeField] private Quaternion RecoilRotation;

    [Space]

    [Header("Crosshair")]

    [SerializeField] RectTransform Crosshair;
    Script_AdvancedMotor Motor;

    RaycastHit hit;

    public Vector3 AimPoint;

    float m_MouseX;
    float m_MouseY;
    float m_Multiplier = 0.01f;

    //Tilt Variables
    float m_XRotation;
    float m_YRotation;
    float m_XCamRotation;
    float m_YCamRotation;

    float m_HeadbobTimer = 0;

    //Shake Variables
    float m_ShakeTime = 0;
    float m_ShakeLerp = 0;
    float m_ShakeAmplitude = 0;
    float m_CurShakeAmplitude = 0;

    //Recoil Variables
    float m_SlerpSpeed = 0;
    float m_RecoilTime = 0;
    float m_RecoilLerp = 0;
    float m_RecoilAngle = 0;
    float m_CurRecoilAngle = 0;

    //Sway Variables
    float m_SwaySlerp = 5;

    //FOV Variables
    float m_MinFov = 80;
    float m_MaxFov = 90;
    float m_CurFov = 80;
    float m_AddFov = 0;
    float m_LerpToFov = 0;
    float m_FovAccel = 5;

    float TiltLerp;

    public float m_CurX;
    float m_LastXRot;

    float CrosshairSize = 0;

    [SerializeField] bool isActive = true;
    public void SetPlayerLookState(bool _isActive){
        isActive = _isActive;
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        m_YRotation = transform.rotation.y;

        Motor = gameObject.GetComponent<Script_AdvancedMotor>();
    }

    private void Update()
    {
        if(isActive)
        {
            m_LastXRot = m_XRotation;

        m_MouseX = Input.GetAxisRaw("Mouse X");
        m_MouseY = Input.GetAxisRaw("Mouse Y");

        Vector2 MouseVec = new Vector2(m_MouseX, m_MouseY);

        m_YRotation += MouseVec.x * SenseX * m_Multiplier;
        m_XRotation -= MouseVec.y * SenseY * m_Multiplier;

        m_XRotation = Mathf.Clamp(m_XRotation, -89.9f, 89.9f);

        TiltLerp = Mathf.Lerp(TiltLerp,Motor.getRawDirection().x, Time.deltaTime * 5);

        CameraHolder.localRotation = Quaternion.Euler(m_XRotation, m_YRotation, -TiltLerp);
        Orientation.transform.rotation = Quaternion.Euler(0, m_YRotation, 0);

        if (Physics.Raycast(MainCam.transform.position, MainCam.transform.forward, out hit, 1000))
        {
            AimPoint = hit.point;
        }
        else
        {
            AimPoint =  MainCam.transform.position + MainCam.transform.forward * 1000;
        }

        Debug.DrawLine(MainCam.transform.position, hit.point, Color.red);


        //Camera Effects
        // Tilt();
        Shake();
        Recoil();
        WeaponSway();
        FOV();


        //Camera Movement
        MoveCam();
        }
        
    }

    void Tilt()
    {

    }

    // Set Camera Sake 
    public void SetShake(float Time, float Amplitude)
    {
        m_ShakeTime = Time;
        m_ShakeLerp = Time;
        m_ShakeAmplitude = Amplitude;
    }

    void Shake()
    {
        if (m_ShakeTime > 0)
        {
            m_ShakeTime-=Time.deltaTime;

            m_CurShakeAmplitude = Mathf.Lerp(0,m_ShakeAmplitude, m_ShakeTime/m_ShakeLerp);

            CameraShaker.localRotation = Quaternion.Euler(CameraShaker.localRotation.x,CameraShaker.localRotation.y,(m_CurShakeAmplitude * Mathf.Sin(m_ShakeTime * 60)));
        }
    }

    public void SetRecoil(float SlerpAcceleration,float Time, Quaternion Rotation)
    {
        m_SlerpSpeed = SlerpAcceleration;
        m_RecoilTime = Time;
        m_RecoilLerp = Time;
        RecoilRotation = CameraRecoiler.localRotation * Rotation;
        RecoilRotation = Gun.localRotation * Rotation;
    }

    void Recoil()
    {
        if (m_RecoilTime > 0)
        {
            m_RecoilTime -=Time.deltaTime;

            m_CurRecoilAngle = Mathf.Lerp(0,m_RecoilAngle, m_RecoilTime/m_RecoilLerp);

            RecoilRotation = Quaternion.Slerp(Quaternion.Euler(m_CurX,0,0),RecoilRotation, m_RecoilTime/m_RecoilLerp);

            if (m_LastXRot < m_XRotation)
            {
                m_CurX = RecoilRotation.x;
            }
        }

        Crosshair.sizeDelta = new Vector2(CrosshairSize * 10, CrosshairSize * 10);

        CameraRecoiler.localRotation = Quaternion.Slerp(CameraRecoiler.localRotation, RecoilRotation ,m_SlerpSpeed  * Time.deltaTime);
        //Gun.localRotation = Quaternion.Slerp(Gun.localRotation, RecoilRotation ,m_SlerpSpeed * Time.deltaTime);
    }

    void MoveCam()
    {
        //Crouching
        if (Motor.GetIsCrouching())
        {
            CameraHolder.localPosition = new Vector3(0,Mathf.Lerp(CameraHolder.localPosition.y, 0.75f, Time.deltaTime * Motor.GetCrouchSlerp()),0);
        }
        else
        {
            CameraHolder.localPosition = new Vector3(0,Mathf.Lerp(CameraHolder.localPosition.y, 1.75f, Time.deltaTime *  Motor.GetCrouchSlerp()),0);
        }
    }

    public void SetWeaponSway(float speed)
    {
        m_SwaySlerp = speed;
    }

    void WeaponSway()
    {
        Quaternion RotX = Quaternion.AngleAxis(-m_MouseY, Vector3.right);
        Quaternion RotY = Quaternion.AngleAxis(m_MouseX, Vector3.up);

        Quaternion TargetRotation = RotX * RotY;
        Gun.localRotation = Quaternion.Slerp(Gun.localRotation, TargetRotation, m_SwaySlerp * Time.deltaTime);
    }

    void FOV()
    {
        if (Motor.GetIsSprinting())
        {
            if (m_CurFov != m_MaxFov)
            {
                m_CurFov = Mathf.Lerp(m_CurFov,m_MaxFov,m_FovAccel * Time.deltaTime);    
            }
        }
        else
        {
            if (m_CurFov != m_MinFov)
            {
                m_CurFov = Mathf.Lerp(m_CurFov,m_MinFov,m_FovAccel * Time.deltaTime);    
            }
        }

        m_AddFov = Mathf.Lerp(m_AddFov,m_LerpToFov,m_FovAccel * Time.deltaTime);   

        MainCam.fieldOfView = m_CurFov + m_AddFov;
    }

    public void SetLerpFov(float FOV)
    {
        m_LerpToFov = FOV;
    }

    public Vector3 getAimPoint()
    {
        return AimPoint;
    }

    public void SetCrosshairSize(float Size)
    {
        CrosshairSize = Size;
    }
}
