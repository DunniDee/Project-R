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
        if(Played == false)
        {
            StartCoroutine(Example1());
            StartCoroutine(Example4());

            Elevatin.speed = prevSpeed;
            
            Played = true;
            Elevatin.Play(GoUp, 0, 0f);
           
            
        }

    



    }

    IEnumerator Example4()
    {
        yield return new WaitForSeconds(10);
        Played = false;
    }

    IEnumerator Example1()
    {

        yield return new WaitForSeconds(6);
        //Animator("Elevator").speed = -1.0;
        var animator = GetComponent<Animator>();
        prevSpeed = animator.speed;
        animator.speed = 0;
        wait = wait ? false : true;



    }

}
