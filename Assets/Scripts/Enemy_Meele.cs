using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Meele : Scr_BaseAI
{
    [SerializeField] float BackOffTime;
    [SerializeField] float AgroRange;
    [SerializeField] float AttackRange;
    [SerializeField] float AttackTime;
    float AttackTimer;
    [SerializeField] float MoveDelay;
    float MoveDelayTimer;

    float xVel;
    float yVel;

    float xVelLerp;
    float yVelLerp;
    public float BackOffTimer;

    bool m_ChaseDoOnce;
    bool m_RunBackDoOnce;
    protected virtual void Seek()
    {
        Agent.isStopped = false;
        Agent.SetDestination(PlayerTransform.position);
        FacePlayer();
        
        if (m_ChaseDoOnce)
        { 
            m_ChaseDoOnce = false;
        }
        yVel = 1;
    }
    protected virtual void BackOff()
    {
        Agent.isStopped = true;

        if (m_RunBackDoOnce)
        { 
            m_RunBackDoOnce = false;
        }
        Agent.Move(-AgentRotator.forward * 2 * Time.deltaTime);
        Agent.Move(AgentRotator.right * xVel * Time.deltaTime);
        FacePlayer();
        yVel = -1;
    }

    protected override void MoveStart()
    {
        FadeAttackUI();
        base.MoveStart();

        xVel = Random.Range(-1,1);

        BackOffTimer = BackOffTime;
        MoveDelayTimer = MoveDelay;
    }

    protected override void MoveUpdate()
    {
        FadeAttackUI();
        base.MoveStart();

        if (MoveDelayTimer < 0)
        {
            if (BackOffTimer > 0)
            {
                m_ChaseDoOnce = true;
                BackOffTimer -= Time.deltaTime;
                BackOff();
            }
            else
            {
                Seek();
                m_RunBackDoOnce = true;
                if ((PlayerTransform.position - Agent.transform.position).sqrMagnitude < AgroRange * AgroRange)
                {
                    m_CurrentState = State.Attack;
                }
            }
        }
        else
        {
            MoveDelayTimer-= Time.deltaTime;
        }

        xVelLerp = Mathf.Lerp(xVelLerp, xVel, Time.deltaTime * 3);
        yVelLerp = Mathf.Lerp(yVelLerp, yVel, Time.deltaTime * 3);

        m_animator.SetFloat("x", xVelLerp);
        m_animator.SetFloat("y", yVelLerp);
    }

    protected override void AttackStart()
    {
        StartAttackUI();
        base.AttackStart();
        AttackTimer = AttackTime;
    }

    protected override void AttackUpdate()
    {
        StartAttackUI();
        if (AttackTimer > 0)
        {
            AttackTimer-= Time.deltaTime;
            base.AttackUpdate();
            FacePlayer();
            Agent.Move((PlayerTransform.position - Agent.transform.position).normalized * 7 * Time.deltaTime);
            Seek();

            if ((PlayerTransform.position - Agent.transform.position).sqrMagnitude < AttackRange * AgroRange)
            {
                m_CurrentState = State.Move;
            }
        }
        else
        {
            m_CurrentState = State.Move;
        }
    }

    protected override void AttackEnd()
    {
        base.AttackEnd();
        Agent.isStopped = true;
        m_animator.SetTrigger("Attack");
    }
    
}
