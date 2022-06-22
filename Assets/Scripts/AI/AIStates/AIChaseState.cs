using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChaseState : AIState
{
    public Transform playerTransform;


    public float attackRange = 4.0f;
    public float attackCurCooldown = 0.0f;
    public float attackMaxCooldown = 2.0f;
    public void IsInRange(Script_BaseAI agent)
    {
        if ((agent.transform.position - agent.GetPlayerTransform().position).magnitude < attackRange)
        {
            Attack(agent);
        }
    }

    void Attack(Script_BaseAI agent)
    {
        int attackIndex = Random.Range(0, 3);
        agent.GetAnimator().SetTrigger("Attack" + attackIndex);
    }

    void AttackFront(Script_BaseAI agent)
    {
        RaycastHit hit;
        if (Physics.SphereCast(agent.transform.position, 1.0f, agent.transform.forward, out hit) && attackCurCooldown <= 0.0f)
        {

            if (hit.transform.CompareTag("Player"))
            {
                Debug.Log("hit" + hit.transform.name);

                hit.transform.GetComponentInParent<Scr_PlayerHealth>().TakeDamage(agent.Config.meleeDamage);
                attackCurCooldown = attackMaxCooldown;
            }

        }
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

        agent.AlertLocalAI(20.0f);
        agent.GetAnimatorEvents().OnAttackEvent += AttackFront;                                 
        playerTransform = agent.GetPlayerTransform();

    }

    public void Update(Script_BaseAI agent)
    {
        if(!agent.enabled){
            return;
        }

        if (attackCurCooldown > 0.0f) { attackCurCooldown -= Time.deltaTime; }
        IsInRange(agent);

        agent.GetNavMeshAgent().destination = playerTransform.position;
    }

    public void Exit(Script_BaseAI agent)
    {

    }
}
