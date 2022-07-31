using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_FlyingShooter : Enemy_Shooter
{
    [SerializeField] Vector3 LerpPos;
    [SerializeField] Transform Drone;
    [SerializeField] protected AudioSource DroneAS;
    protected override void Strafe()
    {
        base.Strafe();
    

        AgentRotator.localPosition = Vector3.Lerp(AgentRotator.localPosition, LerpPos, Time.deltaTime);

        Agent.Move(AgentRotator.right * m_CurrentMoveSpeed * Time.deltaTime);
        AgentRotator.LookAt(PlayerTransform);
    }

    protected override void MoveStart()
    {
        m_MoveSpeed *= -1;
        StateTimer = Random.Range(MoveTimeRange.x,MoveTimeRange.y);
        LerpPos = new Vector3(0,(int)Random.Range(0,6),0);
    }

    protected override void MoveUpdate()
    {
        FadeAttackUI();
        Strafe();
        StateTimer -= Time.deltaTime;
        if (StateTimer < 0.5f)
        {
            m_CurrentMoveSpeed = Mathf.Lerp(m_CurrentMoveSpeed, 0, Time.deltaTime * 4);
            Drone.localRotation = Quaternion.Lerp(Drone.localRotation,Quaternion.Euler(new Vector3(0,0,m_CurrentMoveSpeed * 10)), Time.deltaTime * 4);
            DroneAS.pitch = Mathf.Lerp(DroneAS.pitch,1f, Time.deltaTime);
        }
        else
        {
            m_CurrentMoveSpeed = Mathf.Lerp(m_CurrentMoveSpeed,m_MoveSpeed, Time.deltaTime * 4);
            Drone.localRotation = Quaternion.Lerp(Drone.localRotation,Quaternion.Euler(new Vector3(0,0,-m_CurrentMoveSpeed * 10)), Time.deltaTime * 5);
            DroneAS.pitch = Mathf.Lerp(DroneAS.pitch,1.25f, Time.deltaTime * 5);
        }
    }

    protected override void AttackUpdate()
    {
        StartAttackUI();
        StateTimer -= Time.deltaTime;

        AgentRotator.LookAt(PlayerTransform);
        ShootPos.LookAt(PlayerTransform);
        if (ShootTimer < 0)
        {
            ShootTimer = FireRate; 
            Shoot();
        }
        else
        {
            ShootTimer -= Time.deltaTime;
        }

        Drone.localRotation = Quaternion.Lerp(Drone.localRotation,Quaternion.Euler(Vector3.zero), Time.deltaTime * 5);
    }

    protected virtual void DeadStart()
    {
        Anim.enabled = false;
        m_Ragdoll.ActivateRagdoll();
        m_Ragdoll.ApplyForce(Vector3.up * 10);

        DroneAS.volume = 1;
        DroneAS.pitch = 1.25f;
    }

    protected override void DeadUpdate()
    {
        FadeAttackUI();
        DroneAS.volume = Mathf.Lerp(DroneAS.volume,0, Time.deltaTime  * 2);
        DroneAS.pitch = Mathf.Lerp(DroneAS.pitch,0.25f, Time.deltaTime * 2);
    }
}
