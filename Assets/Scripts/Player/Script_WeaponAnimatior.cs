using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_WeaponAnimatior : MonoBehaviour
{
    [SerializeField] Script_AdvancedMotor Motor;

    float m_xVelocity;
    float m_zVelocity;

    Animator m_Animator;

    public void Shoot(){
        m_Animator.SetTrigger("Shoot");
    }
    public void Reload(){
        m_Animator.SetTrigger("Reload");
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Motor.GetIsSprinting())
        {
            m_zVelocity = Mathf.Lerp(m_zVelocity,Motor.getRawDirection().y * 2, Time.deltaTime * 5);
        }
        else
        {
            m_zVelocity = Mathf.Lerp(m_zVelocity,Motor.getRawDirection().y, Time.deltaTime * 5);
        }
        m_xVelocity = Mathf.Lerp(m_xVelocity,Motor.getRawDirection().x, Time.deltaTime * 5);

        m_Animator.SetFloat("xVelocity", m_xVelocity);
        m_Animator.SetFloat("zVelocity", m_zVelocity);
    }
}
