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
        Script_PlayerStatManager.Instance.Bounty += agent.Config.Bounty;

        for (int i = 0; i < agent.GetUIHealthBar().HealthSlider.maxValue / 100; i++)
        { 
            var go = GameObject.Instantiate(agent.LootPrefab, agent.transform.position + new Vector3(Random.Range(0f, 2f), Random.Range(0,1) , Random.Range(0f, 2f)),Quaternion.identity);
        }

        int rand = Random.Range(0, 1);
        if (rand == 0)
        {
            var go = GameObject.Instantiate(agent.HealthPrefab, agent.transform.position + new Vector3(Random.Range(0f, 2f), Random.Range(0, 1), Random.Range(0f, 2f)), Quaternion.identity);
        }

        ReturnToPool(agent);
    }

    public void Update(Script_BaseAI agent)
    {
        
    }

    public void Exit(Script_BaseAI agent)
    {
       /* GameObject.Destroy(agent.gameObject, 1.0f);*/
    }

    void ReturnToPool(Script_BaseAI agent)
    {
        agent.GetRagdoll().DeactivateRagdoll();
        agent.GetAnimator().enabled = true;
        agent.GetNavMeshAgent().enabled = false;
        agent.GetUIHealthBar().gameObject.SetActive(true);
        agent.GetStateMachine().ChangeState(AIStateID.Idle);
        agent.ResetAgent();



        ObjectPooler.Instance.ReturnObject(agent.gameObject);
    }
}
