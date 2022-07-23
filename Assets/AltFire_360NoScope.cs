using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltFire_360NoScope : Scr_AltFireBase
{
    [SerializeField] Scr_PlayerLook Look;
    [SerializeField] Scr_PlayerMotor Motor;
    float OriginalRotation;
    [SerializeField] AnimationCurve RotationCurve;


    protected override void OnAbilityStart()
    {
        OriginalRotation = Look.m_YRotation;
    }
    
    protected override void OnAbilityDurration()
    {
        base.OnAbilityDurration();

        if (CurrentDurration > Durration / 2)
        {
            Look.m_YRotation = OriginalRotation + Mathf.Lerp(360,0,RotationCurve.Evaluate((CurrentDurration/Durration) - 0.5f) * 2);
        }

        if (Motor.m_VerticalVelocity.y < -1)
        {
           Motor.m_VerticalVelocity.y = -1; 
        }

    }
}
