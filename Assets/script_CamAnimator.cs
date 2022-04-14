using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_CamAnimator : MonoBehaviour
{
    [SerializeField] Script_AdvancedMotor Motor;
    [SerializeField] Animator Rotator;

    bool m_HasLadned = false;

    // Update is called once per frame
    void Update()
    {
        if (Motor.GetIsGrounded())
        {
            if (!m_HasLadned)
            {
                Rotator.SetTrigger("Land");
                m_HasLadned = true;
            }
        }
        else
        {
            if (m_HasLadned)
            {
                Rotator.SetTrigger("Jump");
            }
            m_HasLadned = false;
        }

        Rotator.SetBool("IsSprinting", Motor.GetIsSprinting());
        Rotator.SetBool("IsGrounded", Motor.GetIsGrounded());
    }
}
