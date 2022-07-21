using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_AltFireBase : MonoBehaviour
{
    public enum InputMode
    {
        HoldDown,
        OnPress,
    }
    [SerializeField] protected string AltFireName;
    [SerializeField] protected InputMode CurInputMode;
    [SerializeField] protected float Durration;
    protected float CurrentDurration;
    [SerializeField] protected float Cooldown;
    protected float CurrentCooldown;
    [SerializeField] protected KeyCode AltFireKey = KeyCode.Mouse1;
    protected bool AblityEnd;

    [SerializeField] protected Scr_DiegeticHUD HUD;

    [SerializeField] protected Script_WeaponBase Weapon;

    virtual protected void Update()
    {
        CustomUpdate();

         //Activating Ability
        switch(CurInputMode)
        {
            case InputMode.HoldDown:
                if (Input.GetKey(AltFireKey))
                {
                    if (CurrentCooldown <= Durration)
                    {
                         OnAbilityStart();
                         if(CurrentCooldown < Durration)
                         {
                             CurrentCooldown += Time.deltaTime;
                         }
                        
                    }
                }
                else if(Input.GetKeyUp(AltFireKey))
                {
                    OnAbilityEnd();
                }
                break;
            case InputMode.OnPress:
                if (Input.GetKeyDown(AltFireKey))
                {
                    if (CurrentCooldown <= 0)
                    {
                        OnAbilityStart();
                        CurrentDurration = Durration;
                        CurrentCooldown = Cooldown;
                        AblityEnd = true;
                    }
                }
                break;
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

    protected virtual void CustomUpdate(){}
    protected virtual void OnAbilityStart(){}
    protected virtual void OnAbilityDurration(){}
    protected virtual void OnAbilityEnd(){}

    public float GetCurCooldown()
    {
        return CurrentCooldown;
    }
    public float GetMaxCooldown()
    {
        return Cooldown;
    }
}
