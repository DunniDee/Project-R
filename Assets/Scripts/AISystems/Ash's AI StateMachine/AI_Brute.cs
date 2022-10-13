using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Brute : Script_BaseAI
{
    [Header("Brute Properties")]
    public Scr_CameraEffects CameraEffects;
    public GameObject VFX_Slam;
    public AudioClip SlamAudio;

    /// <summary>
    /// 
    /// </summary>
    protected override void AIStateInit()
    {
        base.AIStateInit();
        AIStateMachine = new Script_AIStateMachine(this);

        AIStateMachine.RegisterState(new AIDeathState());
        AIStateMachine.RegisterState(new AIIdleState());
        AIStateMachine.RegisterState(new AIChaseState());
        AIStateMachine.RegisterState(new AIJumpAttackState());

        AIStateMachine.ChangeState(InitalState);
    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        CameraEffects = FindObjectOfType<Scr_CameraEffects>();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

    }
}
