using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_OnKillBonus : MonoBehaviour
{
    public static Scr_OnKillBonus OnKillBonus;
    [SerializeField] Scr_PlayerMotor Motor;
    [SerializeField] float BonusDurration;
    [SerializeField] float BonusTimer;
    [SerializeField] float DefaultSpeed;
    [SerializeField] float DefaultAirSpeed;

    [SerializeField] AudioClip ChargeUp;
    [SerializeField] AudioClip ChargeDown;

    [SerializeField] AudioSource AS;

    [SerializeField] Scr_SpeedLines m_SpeedLines;

    bool WasCharged;


    public enum KillBonus
    {
        JumpBoost,
        SpeedBoost,
        TimeSlow,
    }

    [SerializeField] KillBonus CurrentKillBonus;
    private void Start() 
    {
        OnKillBonus = this;
        DefaultSpeed = Motor.MoveSpeed;
        DefaultAirSpeed = Motor.AirSpeed;
    }
    public void DoKillBonus()
    {
        BonusTimer = BonusDurration;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (BonusTimer > 0)
        {
            BonusTimer -= Time.deltaTime;
            Motor.MoveSpeed = DefaultSpeed * 1.5f;
            Motor.AirSpeed = DefaultAirSpeed * 1.5f;

            if (!WasCharged)
            {
                AS.PlayOneShot(ChargeUp);
                //SpeedBoostManager.TriggerOn();
            }

            m_SpeedLines.m_IsKillBoosting = true;
            WasCharged = true;
        }
        else
        {
            Motor.MoveSpeed = DefaultSpeed;
            Motor.AirSpeed = DefaultAirSpeed;


            if (WasCharged)
            {
                AS.PlayOneShot(ChargeDown);
            }

            m_SpeedLines.m_IsKillBoosting = false;
            WasCharged = false;
        }
    }
}
