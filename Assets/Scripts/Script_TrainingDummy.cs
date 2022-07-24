using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_TrainingDummy : MonoBehaviour, IDamageable
{
    [SerializeField] float m_Health = 100;
    [SerializeField] Transform Hinge;

    float m_Timer;
    bool IsDead = false;
    public bool Damage(float _Damage, CustomCollider.DamageType _DamageType,Vector3 _direction)
    {
        switch(_DamageType)
        {
            case CustomCollider.DamageType.Critical:
                m_Health -= _Damage * 2;
            break;
            case CustomCollider.DamageType.Normal:
                m_Health -= _Damage;
            break;
        }

        if (m_Health <= 0)
        {
            IsDead = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    void Update()
    {
        if (IsDead)
        {
            m_Timer -= Time.deltaTime;

            Hinge.localRotation = Quaternion.Slerp(Hinge.localRotation,  Quaternion.Euler(170,0,0), Time.deltaTime * 5);

            if (m_Timer <= 0)
            {
                m_Health = 100;
                IsDead = false;
            }
        }
        else
        {
            m_Timer = 3;
            Hinge.localRotation = Quaternion.Slerp(Hinge.localRotation,  Quaternion.Euler(0,0,0), Time.deltaTime * 5);
        }
    }
}
