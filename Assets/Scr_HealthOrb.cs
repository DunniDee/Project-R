using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_HealthOrb : Scr_Credit
{
    protected override void OnPlayerCollided(Collider other) 
    {
        base.OnPlayerCollided(other);
        other.GetComponentInParent<Scr_PlayerHealth>().currentHealth += Value;
        Debug.Log("healing Player " + Value);
    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
