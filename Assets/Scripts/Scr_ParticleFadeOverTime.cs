using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_ParticleFadeOverTime : MonoBehaviour
{
    public float TimeTill0 = 5f;
    [SerializeField] Material Mat;
    [SerializeField] Renderer Rend;
    void Start()
    {
        Mat = Rend.material;


    }

    
    void Update()
    {
        TimeTill0 -= 4f * Time.deltaTime;
        Mat.SetVector("_ParticleFade", new Vector2(1,TimeTill0) );

    }
}
