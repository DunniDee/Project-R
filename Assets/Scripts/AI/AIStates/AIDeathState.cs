using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeathState : AIState
{
    public AIStateID getID()
    {
        return AIStateID.Death;
    }

    public void Enter(Script_BaseAI agent)
    {
        agent.GetRagdoll().ActivateRagdoll();
        agent.GetAnimator().enabled = false;
        agent.GetNavMeshAgent().enabled = false;
        agent.GetUIHealthBar().gameObject.SetActive(false);
        //agent.GetAnimator().SetTrigger("Die");
        UnityEngine.MonoBehaviour.Destroy(agent.gameObject,3.5f);
    }

    public void Update(Script_BaseAI agent)
    {
        
    }

    public void Exit(Script_BaseAI agent)
    {
        
    }
}
