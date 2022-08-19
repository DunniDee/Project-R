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

        agent.GetNavMeshAgent().enabled = true;

    }

    public void Update(Script_BaseAI agent)
    {
        if(!agent.enabled){
            return;
        }
      
    }

    public void Exit(Script_BaseAI agent)
    {
       
    }     
}
