using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script Owner: Ashley Rickit

/// <summary>
/// Ai State ID's
/// </summary>
public enum AIStateID {
    Idle,
    ChasePlayer,
    Death,
    JumpAttack,
}

/// <summary>
///  AI State Interface
/// </summary>
public interface AIState 
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    AIStateID getID();
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="agent"></param>
    void Enter(Script_BaseAI agent);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="agent"></param>
    void Update(Script_BaseAI agent);
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="agent"></param>
    void Exit(Script_BaseAI agent);
}
