using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Brute_ChaseState : AIState
{
    public float attackRange = 2.0f;
    public float attackCurCooldown = 0.0f;
    public float attackMaxCooldown = 1.5f;

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

        if (attackCurCooldown <= 0.0f)
        {
            agent.GetAnimator().SetTrigger("Attack" + attackIndex);
            attackCurCooldown = attackMaxCooldown;
        }
        
    }

    void AttackFront(Script_BaseAI agent)
    {
        RaycastHit hit;
        if (Physics.SphereCast(agent.transform.position, 1.0f, agent.transform.forward, out hit, attackRange))
        {

            if (hit.transform.CompareTag("Player"))
            {
                Debug.Log("hit" + hit.transform.name);

                hit.transform.GetComponentInParent<Scr_PlayerHealth>().TakeDamage(agent.Config.meleeDamage);

            }

        }
    }

    void FarAttack(Script_BaseAI agent)
    {
        RaycastHit hit;
        if (Physics.SphereCast(agent.transform.position, 2.0f, agent.transform.forward, out hit, 2.0f))
        {

            if (hit.transform.CompareTag("Player"))
            {
                Debug.Log("hit" + hit.transform.name);

                hit.transform.GetComponentInParent<Scr_PlayerHealth>().TakeDamage(agent.Config.meleeDamage);

            }

        }
    }

    public void Enter(Script_BaseAI agent)
    {
        agent.GetNavMeshAgent().speed = agent.Config.ChaseSpeed;
        agent.SetIsInCombat(true);
        agent.GetAnimator().SetBool("isInCombat", true);
        agent.GetNavMeshAgent().stoppingDistance = 3.5f;
        agent.AlertLocalAI(20.0f);
        agent.GetAnimatorEvents().OnAttackEvent += AttackFront;
        agent.GetAnimatorEvents().OnFarAttackEvent += FarAttack;
        agent.PlayCombatNoise();
    }

    public void Exit(Script_BaseAI agent)
    {

    }

    public void Update(Script_BaseAI agent)
    {
        if (!agent.enabled)
        {
            return;
        }

        if (attackCurCooldown > 0.0f) { attackCurCooldown -= Time.deltaTime; }
        IsInRange(agent);

        agent.GetNavMeshAgent().destination = agent.GetPlayerTransform().position;
    }

    public AIStateID getID()
    {
        return AIStateID.BruteChase;
    }

}
