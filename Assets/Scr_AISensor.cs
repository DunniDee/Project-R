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

        int segments = 10;
        int numTri = (segments * 4) + 2 + 2;
        int numVert = numTri * 3;

        Vector3[] vertices = new Vector3[numVert];
        int[] triangles = new int[numVert];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0,-angle,0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0,angle,0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;
        int ver = 0;

        // left side
        vertices[ver++] = bottomCenter;
        vertices[ver++] = bottomLeft;
        vertices[ver++] = topLeft;

        vertices[ver++] = topLeft;
        vertices[ver++] = topCenter;
        vertices[ver++] = bottomCenter;

        // right side
        vertices[ver++] = bottomCenter;
        vertices[ver++] = topCenter;
        vertices[ver++] = topRight;

        vertices[ver++] = topRight;
        vertices[ver++] = bottomRight;
        vertices[ver++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;
        for(int i = 0; i < segments; ++i)
        {
            
            bottomLeft = Quaternion.Euler(0,currentAngle,0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0,currentAngle + deltaAngle,0) * Vector3.forward * distance;


            topLeft = bottomLeft + Vector3.up * height;
            topRight = bottomRight + Vector3.up * height;
        
             // far side
            vertices[ver++] = bottomLeft;
            vertices[ver++] = bottomRight;
            vertices[ver++] = topRight;

            vertices[ver++] = topRight;
            vertices[ver++] = topLeft;
            vertices[ver++] = bottomLeft;
            // top
            vertices[ver++] = topCenter;
            vertices[ver++] = topLeft;
            vertices[ver++] = topRight;
        
            // bottom 
            vertices[ver++] = bottomCenter;
            vertices[ver++] = bottomLeft;
            vertices[ver++] = bottomRight;

            currentAngle += deltaAngle;
        }
       

        for(int i = 0; i < numVert; i++){
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate()
    {
        wedgeMesh = CreateWedgeMesh();
    }

    private void OnDrawGizmos()
    {
        if(wedgeMesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(wedgeMesh, transform.position, transform.rotation, transform.localScale);
        }
    }
}
