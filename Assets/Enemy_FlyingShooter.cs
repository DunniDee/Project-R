using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_FlyingShooter : Enemy_Shooter
{
    [SerializeField] Vector3 LerpPos;
    [SerializeField] Transform Drone;
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
        Drone.localRotation = Quaternion.Lerp(Drone.localRotation,Quaternion.Euler(new Vector3(0,0,-m_CurrentMoveSpeed * 10)), Time.deltaTime * 5);
        if (StateTimer < 0.5f)
        {
            m_CurrentMoveSpeed = Mathf.Lerp(m_CurrentMoveSpeed, 0, Time.deltaTime * 4);
        }
        else
        {
            m_CurrentMoveSpeed = Mathf.Lerp(m_CurrentMoveSpeed,m_MoveSpeed, Time.deltaTime * 4);
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
}
