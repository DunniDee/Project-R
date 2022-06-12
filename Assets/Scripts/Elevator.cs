using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Animator Elevatin;

    public string GoUp = "Elevator";

    public bool Played = false;
    public bool wait = false;

    public float prevSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if(Played == false )//&& wait == false)
        {
            Elevatin.speed = prevSpeed;
            StartCoroutine(Example4());
            Played = true;
            Elevatin.Play(GoUp, 0, 0f);
           
            
        }

        if (Played == false)
        {
            StartCoroutine(Example1());
        }



    }

    IEnumerator Example4()
    {
        yield return new WaitForSeconds(6);
        Played = false;
    }

    IEnumerator Example1()
    {

        yield return new WaitForSeconds(3);
        //Animator("Elevator").speed = -1.0;
        var animator = GetComponent<Animator>();
        prevSpeed = animator.speed;
        animator.speed = 0;
        //wait = wait ? false : true;



    }

}
