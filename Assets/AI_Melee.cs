using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Melee : Script_BaseAI
{

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        //Set the Inital State of the AI.
        StateMachine = new Script_AIStateMachine(this);


        StateMachine.RegisterState(new AIDeathState());
        StateMachine.RegisterState(new AIIdleState());
        StateMachine.RegisterState(new AIMoveState());
        StateMachine.RegisterState(new AIChaseState());

        StateMachine.ChangeState(InitalState);
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
