using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_SpeedLines : MonoBehaviour
{
    [SerializeField] Scr_CameraEffects CameraEffects;
    public Vector3 RotateTo;
    [SerializeField] Scr_PlayerMotor Motor;
    [SerializeField] ParticleSystem SpeedLines;
    [SerializeField] AudioSource AS;
    private void Start() 
    {     
        AS = gameObject.GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        RotateTo = Vector3.Lerp(RotateTo,CameraEffects.RotateTo, Time.deltaTime * 5);
        transform.localRotation = Quaternion.Euler(RotateTo);

        //SpeedLines.startLifetime = Motor.m_MomentumMagnuitude;

        if (Motor.m_MomentumMagnuitude < 10)
        {
            SpeedLines.startSpeed = 0;
            AS.volume = Mathf.Lerp(AS.volume,0,Time.deltaTime);
        }
        else
        {
            SpeedLines.startSpeed = Motor.m_MomentumMagnuitude * 5;
            AS.volume = Mathf.Lerp(AS.volume,0.02f,Time.deltaTime);
        }
    
    }
}
