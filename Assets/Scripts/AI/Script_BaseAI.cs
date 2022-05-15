using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Script_BaseAI : MonoBehaviour, IDamageable
{
    [Header("Internal Components")]
    public Transform Player;
    public Rigidbody rigidBody;
    public Script_AIStateMachine StateMachine;

    private NavMeshAgent m_navMeshAgent;
    private Animator m_Animator;
    private Script_Ragdoll m_Ragdoll;
    private Script_UIHealth m_UIHealth;

    [Header("AI Properties")]
    public AIStateID InitalState;
    public AIStateConfig Config;

    [SerializeField] private float m_Health;
    private float m_fDamage = 10f;

    public NavMeshAgent GetNavMeshAgent()
    {
        return m_navMeshAgent;
    }
    public Script_Ragdoll GetRagdoll(){
        return m_Ragdoll;
    }
    public Script_UIHealth GetUIHealthBar(){
        return m_UIHealth;
    }

    public Animator GetAnimator(){
        return m_Animator;
    }

    // public void TakeDamage(float _Damage, CustomCollider.DamageType _DamageType)
    // {
    //     if(StateMachine.currentStateID == AIStateID.Idle)
    //     {
    //         StateMachine.ChangeState(AIStateID.ChasePlayer);
    //     }
    //     switch(_DamageType){
    //         case CustomCollider.DamageType.Critical:
    //             m_Health -= _Damage * 2;
    //             break;
    //         case CustomCollider.DamageType.Normal:
    //             m_Health -= _Damage;
    //             break;
    //     }
    //     if (m_Health <= 0)
    //     {
    //         StateMachine.ChangeState(AIStateID.Death);
    //     }
    // }

        public void Damage(float _Damage, CustomCollider.DamageType _DamageType)
    {
        if(StateMachine.currentStateID == AIStateID.Idle)
        {
            StateMachine.ChangeState(AIStateID.ChasePlayer);
        }
        switch(_DamageType){
            case CustomCollider.DamageType.Critical:
                m_Health -= _Damage * 2;
                break;
            case CustomCollider.DamageType.Normal:
                m_Health -= _Damage;
                break;
        }
        if (m_Health <= 0)
        {
            StateMachine.ChangeState(AIStateID.Death);
        }
    }

    void Locomotion()
    {
        m_Animator.SetFloat("Speed", m_navMeshAgent.velocity.magnitude);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Get Components
        m_Animator = GetComponentInChildren<Animator>();
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        rigidBody = GetComponent<Rigidbody>();
        m_Ragdoll = GetComponent<Script_Ragdoll>();
        m_UIHealth = GetComponentInChildren<Script_UIHealth>();
        
        Player = GameObject.FindGameObjectWithTag("Player").transform;

        //Set the Inital State of the AI.
        StateMachine = new Script_AIStateMachine(this);
        StateMachine.RegisterState(new AIChaseState());
        StateMachine.RegisterState(new AIDeathState());
        StateMachine.RegisterState(new AIIdleState());

        StateMachine.ChangeState(InitalState);
        
        //Set the Max Health and the Slider Values
        m_Health = Config.maxHealth;

        m_UIHealth.HealthSlider.maxValue = Config.maxHealth;
        m_UIHealth.HealthSlider.value = m_Health;
    }

    void UpdateUIHealth(){
        m_UIHealth.HealthSlider.value = m_Health;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_navMeshAgent.enabled){
            StateMachine.Update();
            Locomotion();
        }
        UpdateUIHealth();
    }
}
