using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Grenade : Script_AbilityBase
{
    [SerializeField] float Force;
    [SerializeField] GameObject Grenade;
    Transform Look;
    private void Start()
    {
        Look = transform.GetChild(0);
    }

    protected override void OnAbilityStart()
    {
        base.OnAbilityStart();
        GameObject temp = GameObject.Instantiate(Grenade,Look.position,Look.rotation);
        temp.GetComponent<Rigidbody>().AddForce(temp.transform.forward * Force, ForceMode.VelocityChange);
    }
}
