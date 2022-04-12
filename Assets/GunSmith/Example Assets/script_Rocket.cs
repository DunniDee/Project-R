using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_Rocket : MonoBehaviour
{
    Rigidbody RB;
    private void Start() 
    {
        RB = gameObject.GetComponent<Rigidbody>();
    }
    
    private void LateUpdate()
    {

        transform.rotation = Quaternion.LookRotation(RB.velocity);

    }   

    private void OnCollisionEnter(Collision other) 
    {
        Object.Destroy(gameObject);
    }
}
