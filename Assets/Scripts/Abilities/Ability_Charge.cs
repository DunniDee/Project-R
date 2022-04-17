using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Charge : Script_AbilityBase
{
    [SerializeField] float Force;

    Rigidbody RB;
    Transform Orientation;

    Script_AdvancedMotor Motor;
    private void Start()
    {
        RB = gameObject.GetComponent<Rigidbody>();
        Orientation = transform.GetChild(1);
        Motor = gameObject.GetComponent<Script_AdvancedMotor>();
    }

    protected override void OnAbilityStart()
    {
        base.OnAbilityStart();
        Force = 0;
        Motor.enabled = false;
        RB.velocity = new Vector3(0,RB.velocity.y,0);
    }

    protected override void OnAbilityDurration()
    {
        Force += Time.deltaTime * 2000;
        base.OnAbilityDurration();
        if (Motor.GetIsGrounded())
        {
            RB.AddForce(RB.velocity.normalized * Force * Time.deltaTime, ForceMode.Acceleration);
            RB.AddForce(Orientation.forward * Force * 0.1f * Time.deltaTime, ForceMode.Acceleration);
        }
        else
        {
            RB.AddForce(RB.velocity.normalized * Force * 0.1f * Time.deltaTime, ForceMode.Acceleration);
            RB.AddForce(Orientation.forward * Force * 0.1f * 0.1f * Time.deltaTime, ForceMode.Acceleration);
        }
    }

    protected override void OnAbilityEnd()
    {
        base.OnAbilityEnd();
        Motor.enabled = true;
    }
}
