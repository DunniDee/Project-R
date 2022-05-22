using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIShootState : AIState
{
    public Transform playerTransform;

    float timer;
    float fireRate = 2.0f;
    float m_ProjectileForce = 20.0f;

    void Shoot(Script_BaseAI agent)
    {
        var obj = GameObject.Instantiate(agent.Config.projectile, agent.FiringPoint.position, agent.FiringPoint.rotation);
        obj.GetComponent<Scr_EnemyProjectile>().m_fDamage = agent.Config.projectileDamage;
        obj.GetComponent<Rigidbody>().velocity = agent.FiringPoint.forward * m_ProjectileForce;
        timer = fireRate;
        GameObject.Destroy(obj,5.0f);
        agent.StateMachine.ChangeState(AIStateID.Moving);
    }

    void RotateTowardsPlayer(Script_BaseAI agent)
    {
        Vector3 direction = playerTransform.position - agent.transform.position;
        direction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(direction);
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, rotation, Time.deltaTime * 2.0f);
    }

    //Move the agent to random point
   
    public AIStateID getID()
    {
        return AIStateID.ShootPlayer;
    }

    public void Enter(Script_BaseAI agent)
    {
        agent.GetNavMeshAgent().speed = agent.Config.ChaseSpeed;
        playerTransform = agent.Player;

    }

    public void Update(Script_BaseAI agent)
    {
        if(!agent.enabled){
            return;
        }
        RotateTowardsPlayer(agent);

        if(timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }
        else if (timer <= 0.0f)
        {
            timer = fireRate;
            Shoot(agent);
        }
       
    }

    public void Exit(Script_BaseAI agent)
    {

    }
}
