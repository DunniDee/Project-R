using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scr_AbilityUI : MonoBehaviour
{
    public Slider CooldownSLider;
    [SerializeField] Script_AbilityBase m_Ability;

    // Update is called once per frame
    void Update()
    {
        CooldownSLider.value = 1 - (m_Ability.GetCurCooldown()/m_Ability.GetMaxCooldown());
    }
}
