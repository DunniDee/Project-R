  using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Ragdoll : MonoBehaviour
{
    [SerializeField]
    Rigidbody[] RigidArray;
    [SerializeField] GameObject Hipbone;

    // Start is called before the first frame update
    void Start()
    {
        RigidArray = GetComponentsInChildren<Rigidbody>();
    
        foreach(var rigid in RigidArray){
            rigid.gameObject.AddComponent<CustomCollider>();
        }
        DeactivateRagdoll();
    }

    public void DeactivateRagdoll(){
        foreach(Rigidbody rb in RigidArray){
            rb.isKinematic = true;
        }
    }
    public void ActivateRagdoll(){
        foreach(Rigidbody rb in RigidArray){
            rb.isKinematic = false;
        }
    }
   
   public void ApplyForce(Vector3 _force)
   {
       var rigidBody = Hipbone.GetComponent<Rigidbody>();
       
       rigidBody.AddForce(_force, ForceMode.Impulse);
   }
}
