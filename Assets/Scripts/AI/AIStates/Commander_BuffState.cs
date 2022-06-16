using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Commander_BuffState : AIState
{
    Transform followTarget;

    //Buff Nearby Agents
    void Buff(Script_BaseAI agent)
    {
        agent.GetComponentInChildren<Scr_CommanderSphere>().gameObject.SetActive(true);
    }

    public void Enter(Script_BaseAI agent)
    {
        agent.GetAnimator().SetTrigger("Buff");
        agent.SetIsInCombat(true);
        Buff(agent);


        if (!followTarget)
        {
            RaycastHit hit;
            if (Physics.SphereCast(agent.transform.position, 100.0f, agent.transform.forward, out hit))
            {
                if (hit.transform.CompareTag("Enemy"))
                {
                    followTarget = hit.transform;
                }

            }
        }

        agent.GetNavMeshAgent().SetDestination(followTarget.position);
    }

    public void Exit(Script_BaseAI agent)
    {
        
    }

    public AIStateID getID()
    {
        return AIStateID.CommanderBuff;
    }

    public void Update(Script_BaseAI agent)
    {

    }
}
