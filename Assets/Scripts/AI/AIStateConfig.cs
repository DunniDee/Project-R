using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AIStateConfig : ScriptableObject
{
    [Header("AI Properties")]
    public float m_fMovementSpeed = 4f;
    public float maxHealth = 100f;

    public Vector2 WanderExtents = new Vector2(5.0f, 5.0f);
    
    [Header("Chase Properties")]
    public float ChaseSpeed = 7.0f;
    public float maxTime = 1.0f;
    public float maxDistance = 1.0f;
    
    [Header("Idle Properties")]
    public float WanderSpeed = 2.0f;
    public float maxSightDistance = 5.0f;
    public float wanderRadius = 10.0f;
    
    [Header("Shooting Properties")]
    public GameObject projectile;

    public float projectileDamage = 10f;

    
}
