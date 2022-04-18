using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCollider : MonoBehaviour
{
    public enum DamageType {
        Normal,
        Critical,
    }

    public DamageType damageType;
    public delegate void TakeDamageDelegate(float _Damage, DamageType _DamageType);
    public TakeDamageDelegate takeDamageEvent;

    public void TakeDamage(float _Damage, DamageType _DamageType){
        if(takeDamageEvent != null){
            Debug.Log("Taking Damage");
            takeDamageEvent(_Damage,damageType);
        }
    }

    void Awake(){
        takeDamageEvent += GetComponentInParent<Script_BaseAI>().TakeDamage;
        if(gameObject.tag == "Head"){
            damageType = DamageType.Critical;
        }
    }
}
