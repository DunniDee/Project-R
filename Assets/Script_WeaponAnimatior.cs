using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_WeaponAnimatior : MonoBehaviour
{
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
        
    }
}
