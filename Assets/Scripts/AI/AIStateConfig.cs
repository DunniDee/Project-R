using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AIStateConfig : ScriptableObject
{
    [Header("AI Properties")]
    public float m_fMovementSpeed = 4f;
    public float maxHealth = 100f;
    
    [Header("Chase Properties")]
    public float maxTime = 1.0f;
    public float maxDistance = 1.0f;
    [Header("Idle Properties")]
    public float maxSightDistance = 5.0f;
    
}
