using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AIStateConfig : ScriptableObject
{
    [Header("AI Properties")]
    public float maxHealth = 100f;

    public Vector2 WanderExtents = new Vector2(5.0f, 5.0f);
    
    [Header("Movement Properties")]
    public float ChaseSpeed = 7.0f;
    
    [Header("Shooting Properties")]
    public GameObject projectile;


    [Header("Damage Properties")]
    public Vector2 MeleeDamageExtents = new Vector2(15f,28f);

    public Vector2 ProjectileDamageExtents = new Vector2(10f, 15f);

    [Header("Bounty")]
    public float Bounty = 0.2f;
}
