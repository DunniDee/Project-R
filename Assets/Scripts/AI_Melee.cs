using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Melee : Script_BaseAI
{
    protected override void AIStateInit()
    {
        base.AIStateInit();
        StateMachine = new Script_AIStateMachine(this);


        StateMachine.RegisterState(new AIDeathState());
        StateMachine.RegisterState(new AIIdleState());

        StateMachine.RegisterState(new AIChaseState());

        StateMachine.ChangeState(InitalState);
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
