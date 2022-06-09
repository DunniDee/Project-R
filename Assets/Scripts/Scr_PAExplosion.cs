using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PAExplosion : MonoBehaviour
{
    float Timer = 1;
    float Radius = 0;
    public void setRadius(float _Radius)
    {
        Timer = 1;
        Radius = _Radius;
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime * 5;
        float Lerp = Mathf.Lerp(Radius,0,Timer);
        transform.localScale = new Vector3(Lerp,Lerp,Lerp);

        if (Timer < 0)
        {
            ObjectPooler.Instance.ReturnObject(gameObject);  
        }
    }
}
