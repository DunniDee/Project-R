using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Script_BaseAI : MonoBehaviour
{
    [Header("Internal Components")]
    public NavMeshAgent Agent;
    public Transform Player;
    public Rigidbody rigidBody;
    private Animator m_Animator;
    private Script_Ragdoll m_Ragdoll;
    private Script_UIHealth m_UIHealth;
    

    [Header("AI Properties")]
    private float m_fMovementSpeed = 4f;
    private float maxHealth = 100f;
    [SerializeField] private float m_fHealth;
    private float m_fDamage = 10f;

    public void TakeDamage(float _Damage, CustomCollider.DamageType _DamageType)
    {
        switch(_DamageType){
            case CustomCollider.DamageType.Critical:
                m_fHealth -= _Damage * 2;
                break;
            case CustomCollider.DamageType.Normal:
                m_fHealth -= _Damage;
                break;
        }
        if (m_fHealth <= 0)
        {
            Death();
        }
    }

    void MoveTo(Vector3 target)
    {
        Locomotion();
        Agent.SetDestination(target);
    }
    
    void Locomotion()
    {
        m_Animator.SetFloat("Speed", Agent.velocity.magnitude);
    }

    void Death(){
        m_Ragdoll.ActivateRagdoll();
        m_Animator.enabled = false;
        Agent.enabled = false;
        m_UIHealth.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponentInChildren<Animator>();
        Agent = GetComponent<NavMeshAgent>();
        rigidBody = GetComponent<Rigidbody>();
        m_Ragdoll = GetComponent<Script_Ragdoll>();
        m_UIHealth = GetComponentInChildren<Script_UIHealth>();
        
        m_fHealth = maxHealth;

        m_UIHealth.HealthSlider.maxValue = maxHealth;
        m_UIHealth.HealthSlider.value = m_fHealth;
    }

    void UpdateUIHealth(){
        m_UIHealth.HealthSlider.value = m_fHealth;
    }
    // Update is called once per frame
    void Update()
    {
        if(Agent.enabled){
            MoveTo(Player.position);
        }
        UpdateUIHealth();
    }
}
