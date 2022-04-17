using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Jetpack : Script_AbilityBase
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
        RB.velocity = new Vector3( RB.velocity.x, 2,  RB.velocity.z);
    }

    protected override void OnAbilityDurration()
    {
        base.OnAbilityDurration();
        RB.AddForce(Vector3.up * Force * Time.deltaTime, ForceMode.Acceleration);
        RB.AddForce(Orientation.forward * Force * 0.25f * Time.deltaTime, ForceMode.Acceleration);
    }
}

