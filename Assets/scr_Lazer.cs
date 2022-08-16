using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Lazer : MonoBehaviour
{
    private LineRenderer lineRenderer;

    Vector3 lineStart;
    Vector3 lineEnd;

    
    // Start is called before the first frame update
    void Start()
    {
        lineStart = transform.localPosition;
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.SetPosition(0, transform.localPosition);
        lineRenderer.SetPosition(1, transform.position + lineEnd);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
