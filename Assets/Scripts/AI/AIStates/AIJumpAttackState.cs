using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIJumpAttackState : AIState
{
    float timeJumping = 0.0f;
    public Vector3 initalPosition;
    public Vector3 FinalPosition;
    public AIStateID getID()
    {
        return AIStateID.JumpAttack;
    }

    public void Enter(Script_BaseAI agent)
    {
        agent.GetNavMeshAgent().enabled = false;
        agent.GetRigid().isKinematic = false;
        agent.GetRigid().useGravity = false;
        initalPosition = agent.transform.position;
        FinalPosition = agent.GetPlayerTransform().position;
        timeJumping = 0.0f;
    }

    public void Update(Script_BaseAI agent)
    {
        timeJumping += Time.deltaTime;
     
        if (timeJumping <= 1)
        {
            agent.transform.position = Vector3.Lerp(initalPosition, FinalPosition, timeJumping);

            Debug.Log(FinalPosition);
            agent.GetRotator().localPosition = new Vector3(0.0f, agent.GetJumpCurve().Evaluate(timeJumping), 0.0f);
        }
        if(timeJumping > 1)
        {
            agent.GetStateMachine().ChangeState(AIStateID.ChasePlayer);
        }
    }

    public void Exit(Script_BaseAI agent)
    {
        agent.GetNavMeshAgent().enabled = true;
        agent.GetRigid().isKinematic = true;
        agent.GetRigid().useGravity = true;
    }

}
