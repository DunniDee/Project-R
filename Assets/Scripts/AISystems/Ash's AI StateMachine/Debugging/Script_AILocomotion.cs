using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Script_AILocomotion : MonoBehaviour
{
    [Header("Internal Components")]
    public NavMeshAgent agent;
    [Space]
    [Header("Debug Properties")]
    public bool ShowPath = true;
    public bool ShowDesiredvelocity = true;
    public bool ShowVelocity = true;
    public bool ShowLineSight = true;

    public bool ShowAIPosition = true;

   public 
   
   void OnDrawGizmos(){
        if (ShowAIPosition)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y + 50.0f, transform.position.z));
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y + 50.0f, transform.position.z), 2.0f);
        }
       if(ShowPath){
           Gizmos.color = Color.magenta;
           var agentPath = agent.path;
           Vector3 prevCorner = transform.position;
           foreach(var corner in agentPath.corners){
               Gizmos.DrawLine(prevCorner,corner);
               Gizmos.DrawSphere(corner,0.1f);
               prevCorner = corner;
           }
       }
       if(ShowDesiredvelocity){
           Gizmos.color = Color.red;
           Gizmos.DrawLine(transform.position, transform.position + agent.desiredVelocity);
       }
       if(ShowVelocity){
              Gizmos.color = Color.green;
              Gizmos.DrawLine(transform.position, transform.position + agent.velocity);
       }
   }
}
