using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_DroneAi : MonoBehaviour
{
    public Rigidbody rb;
    public float repelForce = 30f;
    public float repelForceMin = 30f;
    public float repelForceMax = 30f;
    public float range = 2f;
    public float time = 3f;
    public float rotateForce = 30f;
    public float rotateForceMin = 30f;
    public float rotateForceMax = 30f;
    public float Divide = 0.25f;

    private void OnTriggerStay(Collider other)
    {




       // rb.AddForce(transform.position - other.transform.position * Time.deltaTime * repelForce, ForceMode.Acceleration);
        //rb.AddRelativeTorque(repelForce, 0, 0);
        //{  


    }

    private void Start()
    {
        StartCoroutine(Example4());
        rb.AddRelativeForce(Vector3.up * Time.deltaTime * repelForce, ForceMode.Acceleration);
    }


    void Update()
    {
        rb.AddRelativeForce(Vector3.forward * Time.deltaTime * repelForce * Divide, ForceMode.Acceleration);

        if (Physics.Raycast(transform.position, Vector3.down, range))
        {
            rb.AddForce(Vector3.up * Time.deltaTime * repelForce, ForceMode.Acceleration );
            rb.AddTorque(Vector3.up * Time.deltaTime * rotateForce, ForceMode.Acceleration);
        }

        if (Physics.Raycast(transform.position, Vector3.forward, range))
        {
            rb.AddForce(Vector3.up * Time.deltaTime * repelForce, ForceMode.Acceleration);
            rb.AddTorque(Vector3.up * Time.deltaTime * rotateForce, ForceMode.Acceleration);
        }

        if (Physics.Raycast(transform.position, Vector3.left, range))
        {
            rb.AddForce(Vector3.up * Time.deltaTime * repelForce, ForceMode.Acceleration);
            rb.AddTorque(Vector3.up * Time.deltaTime * rotateForce, ForceMode.Acceleration);
        }

        if (Physics.Raycast(transform.position, Vector3.right, range))
        {
            rb.AddForce(Vector3.up * Time.deltaTime * repelForce, ForceMode.Acceleration);
            rb.AddTorque(Vector3.up * Time.deltaTime * rotateForce, ForceMode.Acceleration);
        }

    }


    IEnumerator Example4()
    {

        yield return new WaitForSeconds(time);
        StartCoroutine(Example4());
        repelForce = Random.Range(repelForceMin, repelForceMax);

        yield return new WaitForSeconds(time);
        StartCoroutine(Example4());
        rotateForce = Random.Range(rotateForceMin, rotateForceMax);

    }




















































    /* public Rigidbody Rb;
     public float look;
     public float upon;
     public float my;
     public float works;
     public float and;
     public float despair;
     public bool angry = true;
     public float anger = 10f;
     //im just saying if you got hit by a bus, I dunno if id cry
     void Start()
     {
         despair = (Random.Range(anger, -anger));
         StartCoroutine(Example4());

     }

     IEnumerator Example4()
     {

         yield return new WaitForSeconds(anger);
         angry = !angry;
         Rb.useGravity = !Rb.useGravity;


     }


     void OnTriggerEnter(Collider other)
     {
         //if (target.tag == "GameController")
         //{

         if (angry == false)
         {
             look = (Random.Range(look, my));
             upon = (Random.Range(upon, and));
             despair = (Random.Range(works, despair));

             Rb.AddForce(look, upon, despair);
             Rb.AddTorque(look, upon, despair);

         }

         // }
         //in top replace other with target


     }

     void OnTriggerExit(Collider other)
     {

         if (angry == false)
         {
             Rb.AddForce(-look, -upon, -despair);
             Rb.AddTorque(-look, -upon, -despair);


         }



     }*/

}