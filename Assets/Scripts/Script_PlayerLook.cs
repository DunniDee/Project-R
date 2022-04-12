using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private Transform CameraShaker;
    [SerializeField] private Camera MainCam;
    [Space]

    [Header("Orientation")]
    [SerializeField] private Transform Orientation;

    Script_AdvancedMotor Motor;

    RaycastHit hit;

    public Vector3 AimPoint;

    float m_MouseX;
    float m_MouseY;
    float m_Multiplier = 0.01f;

    float m_XRotation;
    float m_YRotation;
    float m_XCamRotation;
    float m_YCamRotation;

    float m_HeadbobTimer = 0;

    //CamShake
    float m_ShakeTime = 0;
    float m_ShakeLerp = 0;
    float m_ShakeAmplitude = 0;
    float m_CurShakeAmplitude = 0;

    //Recoil
    float m_SlerpSpeed = 0;
    float m_RecoilTime = 0;
    float m_RecoilLerp = 0;
    float m_RecoilAngle = 0;
    float m_CurRecoilAngle = 0;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        m_YRotation = transform.rotation.y;

        Motor = gameObject.GetComponent<Script_AdvancedMotor>();
    }

    private void Update()
    {
        m_MouseX = Input.GetAxisRaw("Mouse X");
        m_MouseY = Input.GetAxisRaw("Mouse Y");

        Vector2 MouseVec = new Vector2(m_MouseX, m_MouseY);

        m_YRotation += MouseVec.x * SenseX * m_Multiplier;
        m_XRotation -= MouseVec.y * SenseY * m_Multiplier;

        m_XRotation = Mathf.Clamp(m_XRotation, -89.9f, 89.9f);

        CameraHolder.localRotation = Quaternion.Euler(m_XRotation, m_YRotation, 0);
        Orientation.transform.rotation = Quaternion.Euler(0, m_YRotation, 0);

        if (Physics.Raycast(MainCam.transform.position, MainCam.transform.forward, out hit, 1000))
        {
            AimPoint = hit.point;
        }
        else
        {
            AimPoint = MainCam.transform.forward * 1000;
        }

        Debug.DrawLine(MainCam.transform.position, hit.point, Color.red);

        tiltCam();
        Shake();
        Recoil();
        //mouse input
        MoveCam();
    }

    void tiltCam()
    {
        if(Motor.getRawDirection().sqrMagnitude != 0 && Motor.GetIsGrounded())
        {
            if (Motor.GetIsSprinting())
            {
                m_HeadbobTimer += Time.deltaTime  * 9;
                CameraShaker.localPosition = new Vector3(0.025f * Mathf.Sin(m_HeadbobTimer),0.025f * Mathf.Sin(m_HeadbobTimer * 2) * Time.deltaTime,0);
            }
            else if (Motor.GetIsCrouching())
            {
                m_HeadbobTimer += Time.deltaTime  * 3;
                CameraShaker.localPosition = new Vector3(0.025f * Mathf.Sin(m_HeadbobTimer),0.025f * Mathf.Sin(m_HeadbobTimer * 2) * Time.deltaTime,0); 
            }
            else
            {
                 m_HeadbobTimer += Time.deltaTime  * 6;
               CameraShaker.localPosition = new Vector3(0.025f * Mathf.Sin(m_HeadbobTimer),0.025f * Mathf.Sin(m_HeadbobTimer * 2) * Time.deltaTime,0); 
            }
        }
        else
        {
            CameraShaker.localPosition = Vector3.Lerp(MainCam.transform.localPosition, Vector3.zero, Time.deltaTime * 5);
            m_HeadbobTimer = 0;
        }

        // m_XCamRotation = Mathf.Lerp(m_XCamRotation, Motor.getRawDirection().x, Time.deltaTime);
        // m_YCamRotation = Mathf.Lerp(m_YCamRotation, Motor.getRawDirection().y, Time.deltaTime);

         //CameraRotator.localRotation = Quaternion.Euler(m_YCamRotation,0,-m_XCamRotation);
    }

    void MoveCam()
    {
        if (Motor.GetIsCrouching())
        {
            CameraHolder.localPosition = new Vector3(0,Mathf.Lerp(CameraHolder.localPosition.y, 0.75f, Time.deltaTime * 3),0);
        }
        else
        {
            CameraHolder.localPosition = new Vector3(0,Mathf.Lerp(CameraHolder.localPosition.y, 1.75f, Time.deltaTime * 3),0);
        }
    }

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

    public void SetRecoil(float SlerpAcceleration,float Time,float XRot, Quaternion Rotation)
    {
        m_SlerpSpeed = SlerpAcceleration;
        m_RecoilTime = Time;
        m_RecoilLerp = Time;
        m_RecoilAngle = XRot;
        
        CameraRotator.localRotation = CameraRotator.localRotation * Rotation;
    }

    void Recoil()
    {
        if (m_RecoilTime > 0)
        {
            m_RecoilTime -=Time.deltaTime;

            m_CurRecoilAngle = Mathf.Lerp(0,m_RecoilAngle, m_RecoilTime/m_RecoilLerp);
        }

        CameraRotator.localRotation = Quaternion.Slerp(CameraRotator.localRotation, Quaternion.Euler(-m_CurRecoilAngle,0,0), m_SlerpSpeed  * Time.deltaTime);
    }

    public Vector3 getAimPoint()
    {
        return AimPoint;
    }
}
