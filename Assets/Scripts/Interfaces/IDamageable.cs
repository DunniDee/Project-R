using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void Damage(float _Damage, CustomCollider.DamageType _DamageType);
}
