using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Ability : ScriptableObject
{
    float Durration;
    float Cooldown;

    public virtual void OnAbilityStart(){}

    public virtual void OnAbilityDurration(){}

    public virtual void OnAbilityEnd(){}
}
