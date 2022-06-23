using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Script_Ragdoll))]
[RequireComponent(typeof(NavMeshAgent))]
public class Script_BaseAI : MonoBehaviour, IDamageable
{
    [Header("Internal Components")]
    protected Transform PlayerTransform;
    protected Rigidbody rigidBody;
    protected Script_AIStateMachine StateMachine;
    protected AIAnimatorEvents AnimatorEvents;


    protected private NavMeshAgent m_navMeshAgent;
    protected private Animator m_Animator;
    protected private Script_Ragdoll m_Ragdoll;
    [SerializeField] private Script_UIHealth m_UIHealth;

    [Header("AI Properties")]
    public AIStateID InitalState;
    public AIStateConfig Config;
    public string AI_Name_Config;

    public string LootPrefabPath;
    public GameObject LootPrefab;
    public GameObject HealthPrefab;

    [SerializeField]
    protected bool isInCombat = false;

    public float StatDamage = 0.0f;
    [SerializeField] protected float m_Health;

    public Transform FiringPoint;
    public GameObject DamageIndicator;

    protected float UITimer = 0.0f;
    protected float dieForce = 100.0f;

    public void AlertLocalAI(float _radius)
    {
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.GetMask("Enemy"))
            {
                Script_BaseAI agent = collider.GetComponentInParent<Script_BaseAI>();
                if (agent != null)
                {

                    Debug.Log(agent + "Alert this AI!!!");

                }
            }
           
        }
    }

    public AIAnimatorEvents GetAnimatorEvents() { return AnimatorEvents; }
    public Script_AIStateMachine GetStateMachine() { return StateMachine;  }
    public Transform GetPlayerTransform() { return PlayerTransform; }
    public Transform GetFiringPoint()
    {
        return FiringPoint;
    }
    public void SetIsInCombat(bool _bool)
    {
        isInCombat = _bool;
    }
    public bool GetIsInCombat()
    {
        return isInCombat;
    }
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

    public void Damage(float _Damage, CustomCollider.DamageType _DamageType, Vector3 _direction)
    {
        
        if (StateMachine.currentStateID == AIStateID.Idle || StateMachine.currentStateID == AIStateID.Moving)
        {
            isInCombat = true;
            if (this is AI_Melee)
            {
                StateMachine.ChangeState(AIStateID.ChasePlayer);
            }
            else if (this is AI_Gun)
            {
                StateMachine.ChangeState(AIStateID.ShootPlayer);
            }
            else if (this is AI_Commander)
            {
                StateMachine.ChangeState(AIStateID.CommanderBuff);
            }
            else if (this is AI_Brute)
            {
                StateMachine.ChangeState(AIStateID.BruteChase);
            }


        }
        switch(_DamageType){
            case CustomCollider.DamageType.Critical:
                m_Health -= _Damage + StatDamage * 2;
                
                break;
            case CustomCollider.DamageType.Normal:
                m_Health -= _Damage + StatDamage;

                break;
        }
        if (m_Health <= 0 && StateMachine.currentStateID != AIStateID.Death)
        {
            StateMachine.ChangeState(AIStateID.Death);
            _direction.y = 0.0f;
            m_Ragdoll.ApplyForce(_direction * dieForce);
        }
       
        UITimer = 5.0f;
        m_UIHealth.gameObject.SetActive(true);
        m_Animator.SetTrigger("Hit");
        AlertLocalAI(10.0f);
    }

    public void ResetAgent()
    {
        isInCombat = false;
        m_Health = Config.maxHealth;
        m_UIHealth.HealthSlider.maxValue = Config.maxHealth;
        m_UIHealth.HealthSlider.value = Config.maxHealth;
        m_UIHealth.gameObject.SetActive(false);
    }
    protected virtual void AIStateInit() { }
    protected void Locomotion()
    {
        m_Animator.SetFloat("Speed", m_navMeshAgent.velocity.magnitude);
    }

    // Start is called before the first frame update
    protected void Start()
    {
        //Get Components
        AnimatorEvents = GetComponentInChildren<AIAnimatorEvents>();
        m_Animator = GetComponentInChildren<Animator>();
        m_UIHealth = GetComponentInChildren<Script_UIHealth>();
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        rigidBody = GetComponent<Rigidbody>();
        m_Ragdoll = GetComponent<Script_Ragdoll>();

        m_UIHealth.Start();

        // Load AI Config
        if (Config = Resources.Load("AI/AIConfig/" + AI_Name_Config) as AIStateConfig)
        {
            Debug.Log("Loaded Config: " + Config.name);
        }
        else
        {
            Debug.LogWarning("Failed to Load AI Config: " + gameObject.name);
        }

        LootPrefab = Resources.Load(LootPrefabPath) as GameObject;
        HealthPrefab = Resources.Load("GameObjects/Loot/HealthCredit") as GameObject;
        //Find Player Transform Reference
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        ///Set the Max Health and the Slider Values
        m_Health = Config.maxHealth;

        m_UIHealth.HealthSlider.maxValue = Config.maxHealth;
        m_UIHealth.HealthSlider.value = Config.maxHealth;
        m_UIHealth.gameObject.SetActive(false);


        AIStateInit();
       

        // Add On Player Found Event Delegate
        GetComponent<Scr_AISensor>().OnPlayerFoundEvent += StateMachine.ChangeState;

         WakeUp();
    }

    protected void UpdateUIHealth(){
        m_UIHealth.HealthSlider.value = m_Health;
    }

    // Update is called once per frame
    protected void Update()
    {
        Debug.Log(Vector3.Distance(transform.position, PlayerTransform.position));
        if (Vector3.Distance(transform.position,PlayerTransform.position) > 30.0f)
        {
            Debug.Log("Skipped Update");
            return;
        }
        if(UITimer > 0.0f)
        {
            UITimer -= Time.deltaTime;
        }
        else if(UITimer <= 0.0f)
        {
            m_UIHealth.gameObject.SetActive(false);
        }


        if(m_navMeshAgent.enabled){
            StateMachine.Update();
            Locomotion();
        }
        UpdateUIHealth();
    }

    public void WakeUp()
    {
        m_navMeshAgent.enabled = false;

        Invoke("EnableNavmesh", 0.25f);
    }

        public void WakeUpDisabled()
    {
        Invoke("EnableNavmesh", 0.25f);
    }

    public void EnableNavmesh()
    {
        m_navMeshAgent.enabled = true;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0.0f, 1.0f, 0.0f) + 2.0f * transform.forward , 2.0f);
    }
}
