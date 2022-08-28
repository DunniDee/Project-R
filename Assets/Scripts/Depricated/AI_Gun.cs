using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Gun : Script_BaseAI
{
    protected override void AIStateInit()
    {
        base.AIStateInit();
        //Set the Inital State of the AI.
        AIStateMachine = new Script_AIStateMachine(this);


        AIStateMachine.RegisterState(new AIDeathState());
        AIStateMachine.RegisterState(new AIIdleState());

        AIStateMachine.ChangeState(InitalState);
    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
