using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltFire_DashReset : Scr_AltFireBase
{
    [SerializeField] Scr_PlayerMotor Motor;
    [SerializeField] Animator Anim;
    protected override void OnAbilityStart()
    {
        base.OnAbilityStart();
        Motor.m_DashCount++;
        Anim.SetTrigger("altfire");
    }
}
