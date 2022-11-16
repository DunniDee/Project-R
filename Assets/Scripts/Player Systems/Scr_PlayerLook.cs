using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlayerLook : MonoBehaviour
{
    [Header("Mouse Sensitivity")]
    [Range(0,500)]
    [SerializeField] private float SenseX;
    [Range(0,500)]
    [SerializeField] private float SenseY;

    [SerializeField] Transform CameraTransform;
    [SerializeField] Transform OrientationTransform;

    [SerializeField] Scr_HandAnimator HandEffects;

    float m_MouseX;
    float m_MouseY;
    float m_Multiplier = 0.01f;

    //Tilt Variables
    float m_XRotation;
    public float m_YRotation;

    public Vector3 LookPoint;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SenseX = PlayerPrefs.GetFloat("MouseSensitivity", 25);
        SenseY = PlayerPrefs.GetFloat("MouseSensitivity", 25);
    }

    // Update is called once per frame
    void Update()
    {
        SenseX = PlayerPrefs.GetFloat("MouseSensitivity", 25);
        SenseY = PlayerPrefs.GetFloat("MouseSensitivity", 25);

        m_MouseX = Input.GetAxisRaw("Mouse X");
        m_MouseY = Input.GetAxisRaw("Mouse Y");

        Vector2 MouseVec = new Vector2(m_MouseX, m_MouseY);

        m_YRotation += MouseVec.x * SenseX * m_Multiplier;
        m_XRotation -= MouseVec.y * SenseY * m_Multiplier;

        m_XRotation = Mathf.Clamp(m_XRotation, -89.9f, 89.9f);


        CameraTransform.localRotation = Quaternion.Euler(m_XRotation, 0,0);
        OrientationTransform.rotation = Quaternion.Euler(0, m_YRotation, 0);

        HandEffects.RotateTo += new Vector3(-m_MouseY, m_MouseX, 0) * 2 * Time.deltaTime;

        RaycastHit Hit;
        LayerMask AllMask = 1|2|3|4|5|6|7|8|9|10|11|12|13|14|15|16;
        if (Physics.Raycast(CameraTransform.position, CameraTransform.forward,out Hit, 1000,AllMask , QueryTriggerInteraction.Ignore))
        {
            LookPoint = Hit.point;
        }
        else
        {
            LookPoint = CameraTransform.position + CameraTransform.forward * 1000;
        }
    }
}
