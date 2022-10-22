using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpTo : MonoBehaviour
{
    public GameObject Obj;
    public Transform Destination;
    public Transform SelfTransform;
    public float X = 0f;
    public float Y = 0f;
    public float RandRange = 1000f;
    public float LookAt1 = 0f;
    public float LookAt2 = -600f;
    public float LookAt3 = 0f;
    public float TurnOnTime = 1f;

    void Start()
    {
        StartCoroutine(Example4());
        StartCoroutine(Example3());
    }

    IEnumerator Example4()
    {
        yield return new WaitForSeconds(2.5f);

        X = Random.Range(RandRange, -RandRange);
        Y = Random.Range(RandRange, -RandRange);
        SelfTransform.transform.position = new Vector3(X, -600f, Y);
        transform.LookAt(new Vector3(LookAt1, LookAt2, LookAt3));
        var lookPos = Destination.position - transform.position;


        StartCoroutine(Example4());



    }

    IEnumerator Example3()
    {
        yield return new WaitForSeconds(TurnOnTime);
        MeshRenderer m = Obj.GetComponent<MeshRenderer>();
        m.enabled = true;
        Animator A = Obj.GetComponent<Animator>();
        A.enabled = true;
    }
}

