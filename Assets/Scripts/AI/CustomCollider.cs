using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCollider : MonoBehaviour
{
    [SerializeField] Scr_OnKillBonus KillBonus;

    public enum DamageType {
        Normal,
        Critical,
    }

    public DamageType damageType;
    public delegate bool TakeDamageDelegate(float _Damage, DamageType _DamageType, Vector3 _direction);
    public TakeDamageDelegate takeDamageEvent;

    public void TakeDamage(float _Damage, DamageType _DamageType, Vector3 _direction)
    {
        if(takeDamageEvent != null)
        {
            Debug.Log("Taking Damage");

            if (takeDamageEvent(_Damage,damageType,_direction))
            {
                Script_HitMarker.current.KillMarker();
                Scr_OnKillBonus.OnKillBonus.DoKillBonus();
            }
            else
            {
                if (_DamageType == DamageType.Normal)
                {
                    Script_HitMarker.current.HitMarker();
                }
                else
                {
                    Script_HitMarker.current.CritMarker();
                }
            }
        
        }
    }

    void Awake()
    {
        takeDamageEvent += GetComponentInParent<IDamageable>().Damage;
        if(gameObject.tag == "Head"){
            damageType = DamageType.Critical;
        }
    }
}
