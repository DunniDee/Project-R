using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpTo : MonoBehaviour
{
    public Transform Destination;
    public Transform SelfTransform;

    // Start is called before the first frame update
    void Start()
    {

            StartCoroutine(Example4());

     
    }

    IEnumerator Example4()
    {

        yield return new WaitForSeconds(2.5f);
        SelfTransform.transform.position = Destination.transform.position;
        StartCoroutine(Example4());



    }
}

