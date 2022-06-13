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
    public delegate void TakeDamageDelegate(float _Damage, DamageType _DamageType, Vector3 _direction);
    public TakeDamageDelegate takeDamageEvent;

    public void TakeDamage(float _Damage, DamageType _DamageType, Vector3 _direction){
        if(takeDamageEvent != null){
            Debug.Log("Taking Damage");
            takeDamageEvent(_Damage,damageType,_direction);

            if (_DamageType == DamageType.Normal)
            {
                Script_HitMarker.current.Hit();
            }
            else
            {
                Script_HitMarker.current.CritHit();
            }
        
        }
    }

    void Awake(){
        takeDamageEvent += GetComponentInParent<IDamageable>().Damage;
        if(gameObject.tag == "Head"){
            damageType = DamageType.Critical;
        }
    }
}
