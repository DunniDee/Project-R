using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIJumpAttackState : AIState
{
    float timeJumping = 0.0f;
    public Vector3 initalPosition;
    public Vector3 FinalPosition;

    private void Attack(Script_BaseAI agent)
    {
        AI_Brute bruteAgent = (AI_Brute)agent;
        bruteAgent.CameraEffects.ShakeTime = 0.25f;
        bruteAgent.CameraEffects.ShakeAmplitude = 2.5f;
        //Add to object pooler Later
        var slamVFX = MonoBehaviour.Instantiate(bruteAgent.VFX_Slam, FinalPosition, Quaternion.identity);
        MonoBehaviour.Destroy(slamVFX, 15.0f);
        Collider[] hit = Physics.OverlapSphere(FinalPosition, 10.0f);
        agent.audioSource.PlayOneShot(bruteAgent.SlamAudio);
        foreach (Collider collider in hit)
        { 
            if (collider.tag == "Player")
            {
                Debug.Log("Hit! :" + collider.name);
                Scr_PlayerMotor motor = collider.GetComponent<Scr_PlayerMotor>();
                motor.m_MomentumDirection = Vector3.up * 30;
                Scr_PlayerHealth playerHealth = collider.GetComponent<Scr_PlayerHealth>();
                float damage = UnityEngine.Random.Range(agent.Config.MeleeDamageExtents.x, agent.Config.MeleeDamageExtents.y);
                playerHealth.TakeDamage(damage, 0.15f, 0.4f);
            }
        }
    }

    public void SetFinalLeapPosition(Script_BaseAI agent)
    {
        Vector3 position = agent.GetPlayerTransform().position;
        RaycastHit hit;
        if (Physics.Raycast(position + (Vector3.up * 3), Vector3.down, out hit, Mathf.Infinity))
        {
            FinalPosition = hit.point;
        }
    }
    public AIStateID getID()
    {
        return AIStateID.JumpAttack;
    }

    public void Enter(Script_BaseAI agent)
    {
        agent.GetAnimatorEvents().OnJumpAttack += Attack;
        agent.GetNavMeshAgent().enabled = false;
        agent.GetRigid().isKinematic = false;
        agent.GetRigid().useGravity = false;
        initalPosition = agent.transform.position;
        SetFinalLeapPosition(agent);
        timeJumping = 0.0f;
    }

   

    public void Update(Script_BaseAI agent)
    {
        timeJumping += Time.deltaTime;
     
        if (timeJumping < 1.8f)
        {
            agent.GetRotator().localPosition = new Vector3(0.0f, agent.GetJumpCurve().Evaluate(timeJumping) * 5f, 0.0f);
            agent.transform.position = Vector3.Lerp(initalPosition, FinalPosition, timeJumping);       
        }
        else if(timeJumping >= 1.8f) // 1.8 seconds = 0.2 for launch while AI is apused
        {
            agent.GetStateMachine().ChangeState(AIStateID.ChasePlayer);
        }
    }

    public void Exit(Script_BaseAI agent)
    {
        // Attack(agent);
        //Make sure rotator isnt fucked
        agent.GetRotator().localPosition = Vector3.zero;
        agent.GetNavMeshAgent().enabled = true;
        agent.GetRigid().isKinematic = true;
        agent.GetRigid().useGravity = true;
    }

}
