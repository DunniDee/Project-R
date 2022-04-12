using System.Collections;
using UnityEngine;

namespace gs_Types
{
    public enum gs_TriggerType
    {
        SemiAuto,
        FullAuto,
        FullAutoRampUp,
        Burst,
        Charge,
    }

    public enum gs_ShotType
    {
        Hitscan,
        Projectile,
    }

    public enum gs_ChargeType
    {
        Damage,
        Accuracy,
        ShotCount,
        Burst,
        Force,
    }

}