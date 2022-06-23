using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class appearOnTouch : MonoBehaviour
{
    public GameObject Bubbles;
    public GameObject VFXSpawn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        


    }


    void OnTriggerEnter(Collider col)
    {
        Debug.Log("Penis Pasta");
        Instantiate(Bubbles, VFXSpawn.transform.position, VFXSpawn.transform.rotation);

    }


}


