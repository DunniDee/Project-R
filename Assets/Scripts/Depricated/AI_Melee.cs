using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Melee : Script_BaseAI
{
    protected override void AIStateInit()
    {
        base.AIStateInit();
        AIStateMachine = new Script_AIStateMachine(this);


        AIStateMachine.RegisterState(new AIDeathState());
        AIStateMachine.RegisterState(new AIIdleState());

        AIStateMachine.RegisterState(new AIChaseState());

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
