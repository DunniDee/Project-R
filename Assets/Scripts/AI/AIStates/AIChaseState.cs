using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChaseState : AIState
{
    public Transform playerTransform;


    public float attackRange = 2.0f;
    public float attackCurCooldown = 0.0f;
    public float attackMaxCooldown = 2.0f;
    float m_ProjectileForce = 40.0f;

    public Vector2 ChaseTimerExtents = new Vector2(3f, 6f);
    public float currentChaseTime = 0.0f;
    public bool isChasing = false;

    public float MaxDistanceThreshold = 30.0f;
    void Attack(Script_BaseAI agent)
    {
        if (isChasing) return;
        float Distance = Vector3.Distance(agent.transform.position, agent.GetPlayerTransform().position);
        if (Distance > MaxDistanceThreshold)
        {
            StartChasing(agent);
        }
        if (attackCurCooldown <= 0.0f)
        {
            int attackIndex = Random.Range(3, 3);

            switch (attackIndex)
            {
                case 1:
                    agent.GetAnimator().SetTrigger("Attack1");
                    break;
                case 2:
                    agent.GetAnimator().SetTrigger("Attack2");
                    break;
                case 3:
                    agent.GetAnimator().SetTrigger("Attack3");
                    agent.GetStateMachine().ChangeState(AIStateID.JumpAttack);
                    break;
            }
            

            attackCurCooldown = attackMaxCooldown;
        }
       
    }

    void SlashAttackHorizontal(Script_BaseAI agent)
    {
        var obj = GameObject.Instantiate(agent.Config.projectile, agent.GetFiringPoint().position, Quaternion.Euler(agent.GetFiringPoint().forward));

        obj.transform.localScale = new Vector3(obj.transform.localScale.y, obj.transform.localScale.x, obj.transform.localScale.z);
        obj.GetComponent<Scr_EnemyProjectile>().m_fDamage = Random.Range(agent.Config.ProjectileDamageExtents.x, agent.Config.ProjectileDamageExtents.y);
        obj.GetComponent<Rigidbody>().velocity = obj.transform.forward * m_ProjectileForce;
        CapsuleCollider collider = obj.GetComponent<CapsuleCollider>();
        collider.direction = 0;

        float height = collider.height;
        float width = collider.radius;
        collider.radius = height;
        collider.height = width;
    }

    void SlashAttackVertical(Script_BaseAI agent)
    {
        var obj = GameObject.Instantiate(agent.Config.projectile, agent.GetFiringPoint().position, Quaternion.Euler(agent.GetFiringPoint().forward));


        obj.GetComponent<Scr_EnemyProjectile>().m_fDamage = Random.Range(agent.Config.ProjectileDamageExtents.x, agent.Config.ProjectileDamageExtents.y);
        obj.GetComponent<Rigidbody>().velocity = obj.transform.forward * m_ProjectileForce;
    }

   

    public void StartChasing(Script_BaseAI agent)
    {
        currentChaseTime = Random.Range(ChaseTimerExtents.x, ChaseTimerExtents.y);
        isChasing = true;
    }

    private void ChaseUpdate(Script_BaseAI agent)
    {
        if (Vector3.Distance(agent.transform.position, agent.GetPlayerTransform().position) < 5.0f)
        {
            currentChaseTime = -1;
        }
        if (currentChaseTime > 0.0f)
        {
            currentChaseTime -= Time.deltaTime;
        }
        else if (currentChaseTime <= 0.0f && isChasing == true)
        {
            isChasing = false;
        }

        agent.GetNavMeshAgent().SetDestination(agent.GetPlayerTransform().position);

    }
   
    public AIStateID getID()
    {
        return AIStateID.ChasePlayer;
    }

    public void Enter(Script_BaseAI agent)
    {
        agent.GetNavMeshAgent().speed = agent.Config.ChaseSpeed;
        agent.SetIsInCombat(true);
        agent.GetAnimator().SetBool("isInCombat", true);
        agent.GetNavMeshAgent().stoppingDistance = 2.5f;

        agent.GetAnimatorEvents().OnSlashHorizontalAttack += SlashAttackHorizontal;
        agent.GetAnimatorEvents().OnSlashVerticalAttack += SlashAttackVertical;
        
        playerTransform = agent.GetPlayerTransform();
        agent.PlayCombatNoise();

    }

    public void Update(Script_BaseAI agent)
    {
        agent.transform.LookAt(new Vector3(agent.GetPlayerTransform().position.x,agent.transform.position.y, agent.GetPlayerTransform().position.z));
        AttackCooldownUpdate();
        Attack(agent);
        ChaseUpdate(agent);
    }

    //Hew fuckin floaty for no reason 
  
    private void AttackCooldownUpdate()
    {
        if (attackCurCooldown > 0.0f)
        {
            attackCurCooldown -= Time.deltaTime;
        }
    }

    public void Exit(Script_BaseAI agent)
    {
        
    }
}
