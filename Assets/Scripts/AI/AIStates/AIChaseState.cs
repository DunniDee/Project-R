using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChaseState : AIState
{
    public Transform playerTransform;


    public float attackRange = 2.0f;
    public float attackCurCooldown = 0.0f;
    public float attackMaxCooldown = 5;
    float m_ProjectileForce = 40.0f;

    public Vector2 ChaseTimerExtents = new Vector2(3f, 6f);
    public float currentChaseTime = 0.0f;
    public bool isChasing = false;

    public float MaxDistanceThreshold = 30;
    void Attack(Script_BaseAI agent)
    {
        if (isChasing) return;
       

        if (attackCurCooldown <= 0.0f)
        {
            attackCurCooldown = attackMaxCooldown;
            int attackIndex = Random.Range(1, 3);

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
            

            
        }
       
    }

    void SlashAttackHorizontal(Script_BaseAI agent)
    {
        var obj = GameObject.Instantiate(agent.Config.projectile, agent.GetFiringPoint().position, Quaternion.identity);

        obj.transform.LookAt(new Vector3(agent.GetPlayerTransform().position.x, obj.transform.position.y, agent.GetPlayerTransform().position.z));
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
        var obj = GameObject.Instantiate(agent.Config.projectile, agent.GetFiringPoint().position, Quaternion.identity);

        obj.transform.LookAt(new Vector3(agent.GetPlayerTransform().position.x,obj.transform.position.y, agent.GetPlayerTransform().position.z));
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

            float Distance = Vector3.Distance(agent.transform.position, agent.GetPlayerTransform().position);
            if (Distance > MaxDistanceThreshold && attackCurCooldown <= 0)
            {
                agent.GetAnimator().SetTrigger("Attack3");
                agent.GetStateMachine().ChangeState(AIStateID.JumpAttack);

                attackCurCooldown = attackMaxCooldown;

            }
            else if (agent.GetNavMeshAgent().pathStatus == NavMeshPathStatus.PathInvalid && attackCurCooldown <= 0)
            {
                agent.GetAnimator().SetTrigger("Attack3");
                agent.GetStateMachine().ChangeState(AIStateID.JumpAttack);
                attackCurCooldown = attackMaxCooldown;
            }

            if (Distance < 5.0f)
            {
                Attack(agent);
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
        if (attackCurCooldown <= 0)
        {
            Attack(agent);
        }
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
