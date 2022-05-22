using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_AISensor : MonoBehaviour
{
    public float distance = 10.0f;
    public float angle = 30.0f;
    public float height = 1;

    public Color meshColor = Color.red;

    Mesh wedgeMesh;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Mesh CreateWedgeMesh()
    {
        Mesh mesh  = new Mesh();

        int numTri = 8;
        int numVert = numTri * 3;

        Vector3[] vertices = new Vector3[numVert];
        int[] triangles = new int[numVert];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0,-angle,0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0,angle,0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * distance;
        Vector3 topRight = bottomRight + Vector3.up * distance;
        int ver = 0;

        // left side

        // right side

        // far side

        // top
        
        // bottom 
    }
}
