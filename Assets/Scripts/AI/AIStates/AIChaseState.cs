using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChaseState : AIState
{
    public Transform playerTransform;
    float timer = 0.0f;

    public AIStateID getID()
    {
        return AIStateID.ChasePlayer;
    }

    public void Enter(Script_BaseAI agent)
    {
        agent.GetNavMeshAgent().speed = agent.Config.ChaseSpeed;
        if(!playerTransform){
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    public void Update(Script_BaseAI agent)
    {
        if(!agent.enabled){
            return;
        }

        timer -= Time.deltaTime;
        if(!agent.GetNavMeshAgent()){
            agent.GetNavMeshAgent().destination = playerTransform.position;
        }

        if(timer < 0.0f){
            Vector3 Dir = (playerTransform.position - agent.GetNavMeshAgent().destination);
            Dir.y = 0.0f;
            if(Dir.sqrMagnitude > agent.Config.maxDistance * agent.Config.maxDistance){
                if(agent.GetNavMeshAgent().pathStatus != NavMeshPathStatus.PathPartial){
                    agent.GetNavMeshAgent().destination = playerTransform.position;
                }
            }
            timer = agent.Config.maxTime;
        }
    }

    public void Exit(Script_BaseAI agent)
    {

    }
}
