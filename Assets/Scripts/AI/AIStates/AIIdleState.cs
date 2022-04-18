using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdleState : AIState
{
    public AIStateID getID()
    {
        return AIStateID.Idle;
    }

    public void Enter(Script_BaseAI agent)
    {
     
    }

    public void Update(Script_BaseAI agent)
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

    public void Exit(Script_BaseAI agent)
    {
       
    }     
}
