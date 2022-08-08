using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shooter : Scr_BaseAI
{
    [SerializeField] protected Transform ShootPos;
    [SerializeField] protected GameObject Projectile;

    [SerializeField] protected float FireRate;

    [SerializeField] protected Vector2 MoveTimeRange;
    [SerializeField] protected Vector2 AttackTimeRange;

    protected float ShootTimer;
    protected float StateTimer;

    protected override void MoveStart()
    {
        m_MoveSpeed *= -1;
        StateTimer = Random.Range(MoveTimeRange.x,MoveTimeRange.y);
        m_CurrentMoveSpeed = 0;
    }

    protected override void MoveUpdate()
    {
        FadeAttackUI();
        Strafe();
        m_CurrentMoveSpeed = Mathf.Lerp(m_CurrentMoveSpeed,m_MoveSpeed, Time.deltaTime * 5);
        StateTimer -= Time.deltaTime;

        if (StateTimer < 0.5f)
        {
            m_CurrentMoveSpeed = Mathf.Lerp(m_CurrentMoveSpeed, 0, Time.deltaTime * 4);
        }
        else
        {
            m_CurrentMoveSpeed = Mathf.Lerp(m_CurrentMoveSpeed,m_MoveSpeed, Time.deltaTime * 4);
        }
    }

    protected override void MoveEnd()
    {

    }


    protected override void AttackStart()
    {
        StateTimer = Random.Range(AttackTimeRange.x,AttackTimeRange.y);
        ShootTimer = FireRate;
    }

    protected override void AttackUpdate()
    {
        StartAttackUI();
        StateTimer -= Time.deltaTime;


        //AgentRotator.LookAt(PlayerTransform);
        FacePlayer();

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
    }

    protected override void AttackEnd()
    {

    }

    protected override void CheckState()
    {   
        if (StateTimer <= 0)
        {
            switch (m_CurrentState)
            {
                case State.Idle:
                    
                break;

                case State.Move:
                    m_CurrentState = State.Attack;
                break;

                case State.Attack:
                    m_CurrentState = State.Move;
                break;

                case State.Dead:
                    
                break;

                default:
                    Debug.Log("AI Start States Broken");
                break;
            }   
        }
    }

    protected virtual void Strafe()
    {
        Agent.Move(AgentRotator.right * m_CurrentMoveSpeed * Time.deltaTime);
        FacePlayer();

         var delta = PlayerTransform.position - transform.position;
        var angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.Euler(0, angle, 0);
    }


    protected void Shoot()
    {

        GameObject Proj = ObjectPooler.Instance.GetObject(Projectile);
        Proj.transform.position = ShootPos.position;
        Proj.transform.rotation = ShootPos.rotation;

        Proj.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Proj.GetComponent<Rigidbody>().AddForce(Proj.transform.forward * 50 ,ForceMode.VelocityChange);
    }
}


