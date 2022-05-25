using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AIStateID {
    Idle,
    ChasePlayer,
    Moving,
    ShootPlayer,
    Death,
}

public interface AIState 
{
    AIStateID getID();
    void Enter(Script_BaseAI agent);
    void Update(Script_BaseAI agent);
    void Exit(Script_BaseAI agent);
}
