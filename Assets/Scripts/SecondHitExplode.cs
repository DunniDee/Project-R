using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondHitExplode : MonoBehaviour
{
    public float Bounces = 1f;
    public GameObject FlamablePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if( Bounces <= 0f)
        {
            Instantiate(FlamablePrefab, transform.position, Quaternion.identity);
            Object.Destroy(gameObject);
            

        }


    }

    private void OnTriggerEnter(Collider other)
    {
        Bounces = Bounces - 1f;

    }

}
