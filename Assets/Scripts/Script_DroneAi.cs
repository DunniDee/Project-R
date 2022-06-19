using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_DroneAi : MonoBehaviour
{
    public Rigidbody Rb;
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



    }

}