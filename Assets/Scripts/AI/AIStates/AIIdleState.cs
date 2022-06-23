using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdleState : AIState
{
    float idleTimer = 0.0f;
    float waitTime = 2.0f;
  
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
                if (agent is AI_Melee)
                {
                    agent.GetStateMachine().ChangeState(AIStateID.ChasePlayer);

                }
                else if (agent is AI_Gun)
                {
                     agent.GetStateMachine().ChangeState(AIStateID.ShootPlayer);
                }
                else if (agent is AI_Brute)
                {
                    agent.GetStateMachine().ChangeState(AIStateID.BruteChase);
                }

                break;
        }
    }

    public void Exit(Script_BaseAI agent)
    {
       
    }     
}
