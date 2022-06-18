using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIShootState : AIState
{
    public Transform playerTransform;

    float RunTimer;
    float ShootTimer;
    float WaitTime = 1.0f;
    float fireRate = 0.5f;
    float m_ProjectileForce = 20.0f;

    void Shoot(Script_BaseAI agent)
    {
        var obj = GameObject.Instantiate(agent.Config.projectile, agent.GetFiringPoint().position, agent.GetFiringPoint().rotation);
        agent.GetAnimator().SetTrigger("Shoot");
        obj.gameObject.transform.LookAt(agent.GetPlayerTransform().position + new Vector3(0.0f,1.0f,0.0f));
        obj.GetComponent<Scr_EnemyProjectile>().m_fDamage = agent.Config.projectileDamage;
        obj.GetComponent<Rigidbody>().velocity = obj.transform.forward  * m_ProjectileForce;
        ShootTimer = fireRate;
        GameObject.Destroy(obj,5.0f);
        
    }

    void RotateTowardsPlayer(Script_BaseAI agent)
    {
        Vector3 direction = playerTransform.position - agent.transform.position;
        direction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(direction);
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, rotation, Time.deltaTime * 2.0f);
        
/*        Debug.Log(rotation);*/
    }

    //Move the agent to random point
   
    public AIStateID getID()
    {
        return AIStateID.ShootPlayer;
    }

    public void Enter(Script_BaseAI agent)
    {
        agent.GetNavMeshAgent().speed = agent.Config.ChaseSpeed;
        agent.SetIsInCombat(true);
        playerTransform = agent.GetPlayerTransform();
        WaitTime = Random.Range(1.0f,3.0f);
        agent.GetAnimator().SetTrigger("StartAim");
    }

    public void Update(Script_BaseAI agent)
    {
        if(!agent.enabled){
            return;
        }
        RotateTowardsPlayer(agent);

        if(ShootTimer > 0.0f)
        {
            ShootTimer -= Time.deltaTime;
        }
        else if (ShootTimer <= 0.0f)
        {
            ShootTimer = fireRate;
            Shoot(agent);
        }
       
       if(RunTimer > 0.0f)
       {
           RunTimer -= Time.deltaTime;
           
       }
       else if(RunTimer <= 0.0f)
       {
            RunTimer = WaitTime;
            agent.GetStateMachine().ChangeState(AIStateID.Moving);
       }
    }

    public void Exit(Script_BaseAI agent)
    {
        agent.GetAnimator().SetBool("IsAiming", false);
    }
}
