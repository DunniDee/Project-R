using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Test : Script_Ability
{
    public override void OnAbilityStart()
    {
        Debug.Log("Wawa");
    }

    public override void OnAbilityDurration()
    {
        Debug.Log("Wewe");
    }

    public override void OnAbilityEnd()
    {
        Debug.Log("Wowo");
    }
}
