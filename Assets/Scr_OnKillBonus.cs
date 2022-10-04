using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_OnKillBonus : MonoBehaviour
{
    public static Scr_OnKillBonus OnKillBonus;
    [SerializeField] Scr_PlayerMotor Motor;
    public enum KillBonus
    {
        JumpBoost,
        SpeedBoost,
        TimeSlow,
    }

    KillBonus CurrentKillBonus;
    private void Start() 
    {
        OnKillBonus = this;
    }
    public void DoKillBonus()
    {

    }
}
