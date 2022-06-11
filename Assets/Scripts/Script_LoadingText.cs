using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_LoadingText : MonoBehaviour
{
    float Timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;

        transform.position = new Vector3(0,1.5f + Mathf.Sin(Timer * 2),10);
    }
}
