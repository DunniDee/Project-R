using System.Net.Mime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Script_AbilityBase : MonoBehaviour
{
    [SerializeField] protected String AbilityName;
    [SerializeField] protected Sprite Icon;
    
    [SerializeField] protected float Durration;
    protected float CurrentDurration;
   [SerializeField] protected float Cooldown;
    protected float CurrentCooldown;
    [SerializeField] protected KeyCode AbilityKey;
    protected bool AblityEnd;

    virtual protected void Update()
    {
                //Activating Ability
        if (Input.GetKeyDown(AbilityKey) && CurrentCooldown <= 0)
        {
            OnAbilityStart();
            CurrentDurration = Durration;
            CurrentCooldown = Cooldown;
            AblityEnd = true;
        }

        //Reducing Cooldown if is over time
        if (CurrentCooldown > 0)
        {
            Debug.Log(CurrentDurration);
            CurrentCooldown -= Time.deltaTime;
        } 

        //Reducing Durration if there is time
        if (CurrentDurration > 0)
        {
            CurrentDurration -= Time.deltaTime;
            Debug.Log(CurrentDurration);
            OnAbilityDurration();
        }
        else if (AblityEnd)
        {
            OnAbilityEnd();
            AblityEnd = false;
        }        
    }

    protected virtual void OnAbilityStart(){}
    protected virtual void OnAbilityDurration(){}
    protected virtual void OnAbilityEnd(){}
}
