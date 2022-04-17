using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_DodgeRoll : Script_AbilityBase
{
    [SerializeField] float Force;

    Rigidbody RB;
    Transform Orientation;
    private void Start()
    {
        RB = gameObject.GetComponent<Rigidbody>();
        Orientation = transform.GetChild(1);
    }

    protected override void OnAbilityStart()
    {
        base.OnAbilityStart();
        RB.AddForce(Orientation.forward * Force, ForceMode.VelocityChange);
    }
}
