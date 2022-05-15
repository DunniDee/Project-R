using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdleState : AIState
{
    private void FieldOfView(Script_BaseAI agent)
    {
        Vector3 PlayerDir = (agent.Player.position - agent.transform.position);
       if(PlayerDir.magnitude > agent.Config.maxSightDistance)
       {
            return;
       }

       Vector3 agentDir = agent.transform.forward;
       PlayerDir.Normalize();

       float dotProduct = Vector3.Dot(PlayerDir,agentDir);
       if(dotProduct > 0.0f)
       {
            agent.StateMachine.ChangeState(AIStateID.ChasePlayer);
       }
    }

    private void Wander(Script_BaseAI agent){
        FieldOfView(agent);
        if(agent.GetNavMeshAgent().remainingDistance < 0.5f){
            agent.GetNavMeshAgent().destination = agent.transform.position + Random.insideUnitSphere * agent.Config.wanderRadius;
        }
    }
    public AIStateID getID()
    {
        return AIStateID.Idle;
    }

    public void Enter(Script_BaseAI agent)
    {
        agent.GetNavMeshAgent().speed = agent.Config.WanderSpeed;
    }

    public void Update(Script_BaseAI agent)
    {
        Wander(agent);
    }

    public void Exit(Script_BaseAI agent)
    {
       
    }     
}
