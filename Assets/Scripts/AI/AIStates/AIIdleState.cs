using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdleState : AIState
{
    float idleTimer = 0.0f;
    float waitTime = 2.0f;
   /* private void FieldOfView(Script_BaseAI agent)
    {
        Vector3 PlayerDir = (agent.GetPlayerTransform().position - agent.transform.position);
       if(PlayerDir.magnitude > agent.Config.maxSightDistance)
       {
            return;
       }

       Vector3 agentDir = agent.transform.forward;
       PlayerDir.Normalize();

       float dotProduct = Vector3.Dot(PlayerDir,agentDir);
       if(dotProduct > 0.0f)
       {
            agent.SetIsInCombat(true);
       }
    }*/

  
    public AIStateID getID()
    {
        return AIStateID.Idle;
    }

    public void Enter(Script_BaseAI agent)
    {
        idleTimer = waitTime;
        agent.GetNavMeshAgent().speed = agent.Config.WanderSpeed;
    }

    public void Update(Script_BaseAI agent)
    {
        if(!agent.enabled){
            return;
        }
        //FieldOfView(agent);
        switch(agent.GetIsInCombat())
        {
            case false:
                if(idleTimer > 0.0f)
                {
                    idleTimer -= Time.deltaTime;
                }
                else if(idleTimer <= 0.0f)
                {
                    idleTimer = waitTime;
                    agent.GetStateMachine().ChangeState(AIStateID.Moving);
                }
                break;
            case true:
                agent.GetStateMachine().ChangeState(AIStateID.ShootPlayer);
                break;
        }
    }

    public void Exit(Script_BaseAI agent)
    {
       
    }     
}
