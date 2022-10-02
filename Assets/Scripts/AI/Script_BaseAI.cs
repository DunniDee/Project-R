using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Script Owner: Ashley Rickit

[RequireComponent(typeof(Script_Ragdoll))]
[RequireComponent(typeof(NavMeshAgent))]
public class Script_BaseAI : MonoBehaviour, IDamageable
{
    [Header("Internal Components")]
    protected Transform m_PlayerTransform;
    protected Rigidbody m_rigidBody;
    protected Script_AIStateMachine AIStateMachine;
    protected AIAnimatorEvents m_AnimatorEvents;
    [SerializeField] protected Transform m_DamagePopUpParent;

    [SerializeField] protected Transform m_RotatorTransform;
    [SerializeField] protected AnimationCurve m_JumpCurve;
    protected float m_Gravity = -9.84f;

    public AudioSource m_audioSource;

    protected private NavMeshAgent m_navMeshAgent;
    protected private Animator m_Animator;
    protected private Script_Ragdoll m_Ragdoll;
    [SerializeField] private Script_UIHealth m_UIHealth;

    [Header("AI Properties")]
    public AIStateID InitalState;
    public AIStateConfig Config;
    public string AI_Name_Config;

    public List<AudioClip> CombatNoise;
    public List<AudioClip> DeathNoise;

    [SerializeField]
    protected bool m_isInCombat = false;

    [SerializeField] protected float m_Health;

    public Transform FiringPoint;
    public GameObject DamageIndicator;

    protected float m_UITimer = 0.0f;
    protected float m_dieForce = 100.0f;

    public delegate void UpdateUIDelegate();
    public event UpdateUIDelegate UpdateUIEvent;

    #region Getters & Setters
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public float GetHealth()
    {
        return m_Health;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public AnimationCurve GetJumpCurve()
    {
        return m_JumpCurve;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Transform GetRotator()
    {
        return m_RotatorTransform;
    }

    /// <summary>
    /// 
    /// </summary>
    public void PlayCombatNoise()
    {
        m_audioSource.PlayOneShot(CombatNoise[Random.Range(0, CombatNoise.Count)]);
    }

    /// <summary>
    /// 
    /// </summary>
    public void PlayDeathNoise()
    {
        m_audioSource.PlayOneShot(DeathNoise[Random.Range(0, DeathNoise.Count)]);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public AIAnimatorEvents GetAnimatorEvents() { return m_AnimatorEvents; }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Script_AIStateMachine GetStateMachine() { return AIStateMachine;  }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Transform GetPlayerTransform() { return m_PlayerTransform; }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Transform GetFiringPoint()
    {
        return FiringPoint;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Rigidbody GetRigid()
    {
        return m_rigidBody;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_bool"></param>
    public void SetIsInCombat(bool _bool)
    {
        m_isInCombat = _bool;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool GetIsInCombat()
    {
        return m_isInCombat;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public NavMeshAgent GetNavMeshAgent()
    {
        return m_navMeshAgent;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Script_Ragdoll GetRagdoll(){
        return m_Ragdoll;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Script_UIHealth GetUIHealthBar(){
        return m_UIHealth;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Animator GetAnimator(){
        return m_Animator;
    }

    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_Damage"></param>
    /// <param name="_DamageType"></param>
    /// <param name="_direction"></param>
    /// <returns></returns>
    public bool Damage(float _Damage, CustomCollider.DamageType _DamageType, Vector3 _direction)
    {
        if (AIStateMachine.currentStateID == AIStateID.Idle)
        {
            m_isInCombat = true;
            if (this is AI_Brute)
            {
                AIStateMachine.ChangeState(AIStateID.ChasePlayer);
            }


        }
        float totalDamage = _Damage;
        switch (_DamageType){
            case CustomCollider.DamageType.Critical:
               
                m_Health -= totalDamage * 2;
                Scr_DamagePopupManager.Instance.DisplayDamagePopup((int)totalDamage * 2, m_DamagePopUpParent);
                if (UpdateUIEvent != null)
                {
                    UpdateUIEvent();
                }
                break;
            case CustomCollider.DamageType.Normal:
                m_Health -= totalDamage;
                Scr_DamagePopupManager.Instance.DisplayDamagePopup((int)totalDamage, m_DamagePopUpParent);
                if (UpdateUIEvent != null)
                {
                    UpdateUIEvent();
                }
                break;
        }
        if (m_Health <= 0 && AIStateMachine.currentStateID != AIStateID.Death)
        {
            AIStateMachine.ChangeState(AIStateID.Death);
            _direction.y = 0.0f;
            m_Ragdoll.ApplyForce(_direction * m_dieForce);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void ResetAgent()
    {
        m_isInCombat = false;
        m_Health = Config.maxHealth;
        m_UIHealth.HealthSlider.maxValue = Config.maxHealth;
        m_UIHealth.HealthSlider.value = Config.maxHealth;
        m_UIHealth.gameObject.SetActive(false);

        m_navMeshAgent.enabled = false;
    }
    /// <summary>
    /// 
    /// </summary>
    protected virtual void AIStateInit() { }
    
    /// <summary>
    /// 
    /// </summary>
    protected void DebugLocomotion()
    {
        m_Animator.SetFloat("Speed", m_navMeshAgent.velocity.magnitude);
    }

    protected void UpdateUIHealth(){
        m_UIHealth.HealthSlider.value = m_Health;
    }

    /// <summary>
    /// 
    /// </summary>
    public void EnableNavmeshAgent()
    {
        m_navMeshAgent.enabled = true;
    }
    
    // Start is called before the first frame update
    protected void Start()
    {
        //Get Components
        m_AnimatorEvents = GetComponentInChildren<AIAnimatorEvents>();
        m_Animator = GetComponentInChildren<Animator>();
        m_UIHealth = GetComponentInChildren<Script_UIHealth>();
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_rigidBody = GetComponent<Rigidbody>();
        m_Ragdoll = GetComponent<Script_Ragdoll>();
        m_audioSource = GetComponent<AudioSource>();

        // Load AI Config from resources
        if (Config = Resources.Load("AI/AIConfig/" + AI_Name_Config) as AIStateConfig)
        {
            Debug.Log("Loaded Config: " + Config.name);
        }
        else
        {
            Debug.LogWarning("Failed to Load AI Config: " + gameObject.name);
        }

        //Find Player Transform Reference
        m_PlayerTransform = FindObjectOfType<Scr_PlayerMotor>().gameObject.transform;

        ///Set the Max Health and the Slider Values
        m_Health = Config.maxHealth;

        AIStateInit();

        // Add On Player Found Event Delegate
       // GetComponent<Scr_AISensor>().OnPlayerFoundEvent += AIStateMachine.ChangeState;
    }


    // Update is called once per frame
    protected void Update()
    {
        AIStateMachine.Update();
        DebugLocomotion();
    }
}
