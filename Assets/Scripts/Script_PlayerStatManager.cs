using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerStatManager : MonoBehaviour
{

    //Singleton Pattern
    public static Script_PlayerStatManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        } else {
            Instance = this;
          	DontDestroyOnLoad(gameObject);
        }
    }

    // public struct Stat
    // {
    //     public Stat(float _stat)
    //     {
    //         s_default = _stat;
    //         s_Modified = _stat;
    //     }
    //     public float s_default { get; set; }
    //     public float s_Modified { get; set; }
    // }
    // Start is called before the first frame update

    // [SerializeField] Stat Firerate;

    //A bunch of Stats

    [Header("Bounty variables")]
    public float Bounty;
    public float Credits;

    //Movement Variables
    [Header("Movement variables")]
    public float DefaultWalkSpeed;
    public float ModifiedWalkSpeed;
    [Space]
    public float DefaultSprintSpeed;
    public float ModifiedSprintSpeed;
    [Space]
    public float DefaultCrouchSpeed;
    public float ModifiedCrouchSpeed;
    [Space]
    public int DefaultJumpCount;
    public int ModifiedJumpCount;

    [Header("Primary Weapon variables")]
    public float DefaultPrimaryDamage;
    public float ModifiedPrimaryDamage;
    [Space]
    public float DefaultPrimaryFireRate;
    public float ModifiedPrimaryFireRate;
    [Space]
    public int DefaultPrimaryMagCount;
    public int ModifiedPrimaryMagCount;

    [Header("Secondary Weapon variables")]
    public float DefaultSecondaryDamage;    
    public float ModifiedSecondaryDamage;
    [Space]
    public float DefaultSecondaryFireRate;
    public float ModifiedSecondaryFireRate;
    [Space]
    public int DefaultSecondaryMagCount;
    public int ModifiedSecondaryMagCount;
}

