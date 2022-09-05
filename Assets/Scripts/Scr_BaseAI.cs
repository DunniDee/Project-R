using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class Scr_BaseAI : MonoBehaviour, IDamageable
{
    public enum State
    {
        Idle,
        Move,
        Attack,
        Dead,
    }

    [SerializeField]
    public State m_CurrentState;
    protected State m_LastState;

    [SerializeField]
    protected NavMeshAgent Agent;
    [SerializeField]
    protected Transform AgentRotator;
    [SerializeField]
    protected Transform PlayerTransform;

    [SerializeField]
    protected float m_Health;

    [SerializeField]
    protected float m_MoveSpeed;
    protected float m_CurrentMoveSpeed;

    [SerializeField]
    protected Script_Ragdoll m_Ragdoll;
    [SerializeField] protected Animator m_animator;
    [SerializeField] protected Transform AlertPos;
    [SerializeField] protected TextMeshPro AlertText;
    [SerializeField] protected AudioSource AS;
    [SerializeField] protected Transform DamagePopupPos;


    protected void Start() 
    {
        Debug.Log(Script_PlayerStatManager.Instance.PlayerTransform);
        PlayerTransform = Script_PlayerStatManager.Instance.PlayerTransform; // get player transform
    }

    //base classes for the behaviours
    protected void StartAttackUI()
    {
        AlertPos.LookAt(PlayerTransform);
        AlertPos.localRotation *= Quaternion.Euler(Random.Range(-2,2),Random.Range(-2,2),Random.Range(-2,2));
        AlertText.color = new Color(1,0,0,1);

    }

    protected void FadeAttackUI()
    {
        AlertPos.LookAt(PlayerTransform);
        AlertText.color = Color.Lerp(AlertText.color, new Color(0,0,0,0), Time.deltaTime * 10);
    }


    public bool Damage(float _Damage, CustomCollider.DamageType _DamageType,Vector3 _direction)
    {
        switch(_DamageType)
        {
            case CustomCollider.DamageType.Critical:
                m_Health -= _Damage * 2;
                Scr_DamagePopupManager.Instance.DisplayDamagePopup((int)_Damage * 2, DamagePopupPos);
                Scr_DamagePopupManager.Instance.CreateHealthOrb(this.gameObject.transform);
                Scr_StyleManager.i.IncreaseStylePoints(50.0f);
                break;
            case CustomCollider.DamageType.Normal:
                m_Health -= _Damage;
                Scr_DamagePopupManager.Instance.DisplayDamagePopup((int)_Damage, DamagePopupPos);
                Scr_StyleManager.i.IncreaseStylePoints(10.0f);
            break;
        }

        if (m_Health <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Health <= 0) // enemy dead
        {
           m_CurrentState = State.Dead; 
        }

        switch (m_CurrentState)
        {
            case State.Idle:
                IdleUpdate();
            break;

            case State.Move:
                MoveUpdate();
            break;

            case State.Attack:
                AttackUpdate();
            break;

            case State.Dead:
                DeadUpdate();
            break;

            default:
                Debug.Log("AI States are broken");
            break;
        }
        CheckState();
        CheckStateChange();
    }

    void CheckStateChange()
    {
        if (m_CurrentState != m_LastState) // check to see if there are any changes
        {
            // last state on end
            switch (m_LastState)
            {
                case State.Idle:
                    IdleEnd();
                break;

                case State.Move:
                    MoveEnd();
                break;

                case State.Attack:
                    AttackEnd();
                break;

                case State.Dead:
                    DeadEnd();
                break;

                default:
                    Debug.Log("AI End States Broken");
                break;
            }


            // new state on start
            switch (m_CurrentState)
            {
                case State.Idle:
                    IdleStart();
                break;

                case State.Move:
                    MoveStart();
                break;

                case State.Attack:
                    AttackStart();
                break;

                case State.Dead:
                    DeadStart();
                break;

                default:
                    Debug.Log("AI Start States Broken");
                break;
            }
        }

        m_LastState = m_CurrentState;
    }

    protected virtual void CheckState()
    {

    }

    protected virtual void IdleStart()
    {

    }
    protected virtual void IdleUpdate()
    {

    }
    protected virtual void IdleEnd()
    {

    }

    protected virtual void MoveStart()
    {

    }
    protected virtual void MoveUpdate()
    {

    }
    protected virtual void MoveEnd()
    {

    }

    protected virtual void AttackStart()
    {

    }
    protected virtual void AttackUpdate()
    {

    }
    protected virtual void AttackEnd()
    {

    }

    protected virtual void DeadStart()
    {
        m_animator.enabled = false;
        m_Ragdoll.ActivateRagdoll();
        m_Ragdoll.ApplyForce(Vector3.up * 10);
    }

    protected float DeadTimer = 2;
    protected virtual void DeadUpdate()
    {
        FadeAttackUI();
        if (DeadTimer > 0)
        {
            DeadTimer -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void DeadEnd()
    {
        
    }

    protected void FacePlayer()
    {
        Vector3 LookPos = PlayerTransform.position;
        LookPos.y = AgentRotator.position.y;

        AgentRotator.LookAt(LookPos);
    }
}
