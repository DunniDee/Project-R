using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltFire_360NoScope : Scr_AltFireBase
{
    [SerializeField] Scr_PlayerLook Look;
    [SerializeField] Scr_PlayerMotor Motor;
    float OriginalRotation;
    [SerializeField] AnimationCurve RotationCurve;

    [SerializeField] float JumpForce;

    [SerializeField] bool IsHorizontal;

    [SerializeField] Animator Anim;


    protected override void OnAbilityStart()
    {

        OriginalRotation = Look.m_YRotation;

        Motor.m_VerticalVelocity.y = JumpForce;
        Motor.MovePlayer(new Vector3(0, 0.25f,0));

        Anim.SetTrigger("altfire");
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
