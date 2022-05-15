using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIShootState : AIState
{
    public Transform playerTransform;

    float shootTimer = 0.0f;
    float m_ProjectileForce = 100.0f;
    
    public AIStateID getID()
    {
        return AIStateID.ChasePlayer;
    }

    public void Enter(Script_BaseAI agent)
    {
        agent.GetNavMeshAgent().speed = agent.Config.ChaseSpeed;
        playerTransform = agent.Player;

    }

    public void Update(Script_BaseAI agent)
    {
        if(!agent.enabled){
            return;
        }
        agent.transform.LookAt(playerTransform, Vector3.up);

       
    }

    public void Exit(Script_BaseAI agent)
    {

    }
}
