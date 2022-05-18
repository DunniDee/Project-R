using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMoveState : AIState
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
            agent.SetIsInCombat(true);
       }
    }

    //overrides
    public AIStateID getID()
    {
        return AIStateID.Moving;
    }

    void MoveToPosition(Script_BaseAI agent)
    {
        agent.GetNavMeshAgent().SetDestination(agent.transform.position +
         new Vector3(Random.Range(-agent.Config.WanderExtents.x, agent.Config.WanderExtents.y), 0, Random.Range(-agent.Config.WanderExtents.x, agent.Config.WanderExtents.y)));
    }
    //Chose random point to move to
    public void Enter(Script_BaseAI agent)
    {
         MoveToPosition(agent);
    }

    public void Update(Script_BaseAI agent)
    {
        FieldOfView(agent);
        switch(agent.GetIsInCombat())
        {
            case false:
                if(agent.transform.position == agent.GetNavMeshAgent().destination)
                {
                     agent.StateMachine.ChangeState(AIStateID.Idle);
                }
                break;
            case true:
                if(agent.transform.position == agent.GetNavMeshAgent().destination)
                {
                    agent.StateMachine.ChangeState(AIStateID.ShootPlayer);
                }
                break;
        }
       
    }

    public void Exit(Script_BaseAI agent)
    {
        
    }

}
