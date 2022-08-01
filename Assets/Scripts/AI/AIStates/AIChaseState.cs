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
    float m_ProjectileForce = 30.0f;

    void Attack(Script_BaseAI agent)
    {
        if (attackCurCooldown <= 0.0f)
        {
            int attackIndex = Random.Range(1, 4);


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
                    break;
            }
            

            attackCurCooldown = attackMaxCooldown;
        }
       
    }

    void SlashAttackHorizontal(Script_BaseAI agent)
    {
        var obj = GameObject.Instantiate(agent.Config.projectile, agent.GetFiringPoint().position, agent.GetFiringPoint().rotation);
        Scr_PlayerMotor motor = agent.GetPlayerTransform().GetComponent<Scr_PlayerMotor>();
        obj.gameObject.transform.LookAt(agent.GetPlayerTransform().position + motor.m_MomentumDirection);
        obj.transform.localScale = new Vector3(obj.transform.localScale.y, obj.transform.localScale.x, obj.transform.localScale.z);
        obj.GetComponent<Scr_EnemyProjectile>().m_fDamage = agent.Config.projectileDamage;
        obj.GetComponent<Rigidbody>().velocity = obj.transform.forward * m_ProjectileForce;
    }
    void SlashAttackVertical(Script_BaseAI agent)
    {
        var obj = GameObject.Instantiate(agent.Config.projectile, agent.GetFiringPoint().position, agent.GetFiringPoint().rotation);
        obj.gameObject.transform.LookAt(agent.GetPlayerTransform().position + new Vector3(0.0f, 1.0f, 0.0f));
       
        obj.GetComponent<Scr_EnemyProjectile>().m_fDamage = agent.Config.projectileDamage;
        obj.GetComponent<Rigidbody>().velocity = obj.transform.forward * m_ProjectileForce;
    }

    void JumpSlashAttack(Script_BaseAI agent)
    {
        agent.GetNavMeshAgent().isStopped = true;
        agent.GetRigid().AddForce(agent.transform.forward + agent.transform.up * 200.0f);
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
        agent.GetAnimatorEvents().OnJumpAttack += JumpSlashAttack;

        playerTransform = agent.GetPlayerTransform();
        agent.PlayCombatNoise();
    }

    public void Update(Script_BaseAI agent)
    {
        agent.transform.LookAt(agent.GetPlayerTransform());
        AttackCooldownUpdate();
        Attack(agent);
        //agent.GetNavMeshAgent().destination = playerTransform.position;
    }

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
