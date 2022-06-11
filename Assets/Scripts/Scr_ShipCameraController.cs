using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Scr_ShipCameraController : MonoBehaviour
{
    public CinemachineVirtualCamera vcam_StartingPOV;
    public CinemachineVirtualCamera vcam_PlayerPOV;

    public void SwitchPOV(CinemachineVirtualCamera _vcam_NewPOV, CinemachineVirtualCamera _vcam_OldPOV)
    {
        _vcam_NewPOV.Priority = _vcam_OldPOV.Priority; 
    }

    // Start is called before the first frame update
    void Start()
    {
        SwitchPOV(vcam_PlayerPOV, vcam_StartingPOV);    
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchPOV(vcam_StartingPOV, vcam_PlayerPOV);
        }
    }
}
