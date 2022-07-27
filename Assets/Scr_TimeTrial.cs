using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_TimeTrial : MonoBehaviour
{
    public float Timer = 0.0f;
    bool isTrialStarted = false;

    public void StartTimer()
    {
        isTrialStarted = true;
    }

    public void StopTimer()
    {
        isTrialStarted = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTrialStarted)
        {
            Timer += Time.deltaTime;
        }

    }
}
