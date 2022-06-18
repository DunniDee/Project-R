using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_AIStateMachine
{
    public AIState[] States;
    public Script_BaseAI agent;
    public AIStateID currentStateID;
    public Script_AIStateMachine(Script_BaseAI _agent){
        this.agent = _agent;
        int numStates = System.Enum.GetNames(typeof(AIStateID)).Length;
        States = new AIState[numStates];
    }

    public void RegisterState(AIState state)
    {
/*        Debug.Log(agent.name + " Changing to State" + state.getID()); */
        int index = (int)state.getID();
        States[index] = state;
    }

    public AIState GetState(AIStateID stateID){
        return States[(int)stateID]; 
    }
    
    public void Update(){
        GetState(currentStateID)?.Update(agent);
    }

    public void ChangeState(AIStateID newStateID){
        GetState(currentStateID)?.Exit(agent);
        currentStateID = newStateID;
        GetState(currentStateID)?.Enter(agent);
    }
     
}
