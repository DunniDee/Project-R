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

    bool WasCharged;

    [SerializeField] GameObject SpeedBoostParticles;


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
            }

            SpeedBoostParticles.SetActive(true);

            WasCharged = true;
        }
        else
        {
            Motor.MoveSpeed = DefaultSpeed;
            Motor.AirSpeed = DefaultAirSpeed;

            SpeedBoostParticles.SetActive(false);


            if (WasCharged)
            {
                AS.PlayOneShot(ChargeDown);
            }


            WasCharged = false;
        }
    }
}
