using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_Arrow : MonoBehaviour
{
    Rigidbody RB;

    bool m_isStuck = false;
    private void Start() 
    {
        RB = gameObject.GetComponent<Rigidbody>();
    }
    
    private void LateUpdate()
    {
        if (!m_isStuck)
        {
            transform.rotation = Quaternion.LookRotation(RB.velocity);
        }
    }

    private void OnCollisionEnter(Collision other) 
    {
        transform.parent = other.transform.parent;
        RB.isKinematic = true;
        m_isStuck = true;
    }
}
