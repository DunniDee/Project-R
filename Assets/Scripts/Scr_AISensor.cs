using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_AISensor : MonoBehaviour
{

    [Header("Sensor Properties")]
    public float distance = 10.0f;
    public float angle = 30.0f;
    public float height = 1;
    public Color meshColor = Color.red;

    public float scanFrequency = 30.0f;
    public LayerMask Layers;

    [Header("Collider Properties")]
    public List<GameObject> Objects = new List<GameObject>();
    Collider[] Colliders = new Collider[50];
    [Header("Internal Components")]
    [SerializeField] Script_BaseAI agent;
    public delegate void OnPlayerFoundDelegate(AIStateID _State);
    public OnPlayerFoundDelegate OnPlayerFoundEvent;

    Mesh wedgeMesh;
    int count;
    float scanInterval;
    public float scanTimer;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<Script_BaseAI>();
        CreateWedgeMesh();
        scanFrequency = 1 / scanFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        scanTimer -= Time.deltaTime;
        if(scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();
        }
    }

    private void Scan()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, Colliders,Layers, QueryTriggerInteraction.Collide);
        Objects.Clear();
        for(int i =0; i < count; i++)
        {
            GameObject obj = Colliders[i].gameObject;
            if(IsInSight(obj))
            {
                Objects.Add(obj);
            }
        }
        if (agent is AI_Commander && agent.GetIsInCombat())
        {
            agent.GetNavMeshAgent().destination = Objects[0].transform.position;
        }
        foreach (GameObject gameObject in Objects)
        {
            
           
            if (gameObject.CompareTag("Player"))
            {

                // Play Found Event
                if (agent is AI_Melee)
                {
                    if (OnPlayerFoundEvent != null && !agent.GetIsInCombat())
                    {
                        OnPlayerFoundEvent(AIStateID.ChasePlayer);
                    }
                }
                else if (agent is AI_Gun)
                {
                    if (OnPlayerFoundEvent != null && !agent.GetIsInCombat())
                    {
                        OnPlayerFoundEvent(AIStateID.ShootPlayer);
                    }
                }
                else if (agent is AI_Commander)
                {
                    if (OnPlayerFoundEvent != null && !agent.GetIsInCombat())
                    {
                        OnPlayerFoundEvent(AIStateID.CommanderBuff);
                    }
                }


            }
        }
    }

    public bool IsInSight(GameObject obj)
    {
        Vector3 origin = transform.position;
        Vector3 dest = obj.transform.position;
        Vector3 direction = dest - origin;
        if(direction.y < -1.5 || direction.y > height)
        {
            return false;
        }
        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        if(deltaAngle > angle)
        {
            return false;
        }
        
        return true;
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
        scanInterval = 1.0f/ scanFrequency;
    }

    private void OnDrawGizmos()
    {
        if(wedgeMesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(wedgeMesh, transform.position, transform.rotation, transform.localScale);
        }

        Gizmos.DrawWireSphere(transform.position, distance);
        for(int  i = 0; i < count; i++)
        {
            Gizmos.DrawSphere(Colliders[i].transform.position, 0.2f);
        }

        Gizmos.color = Color.green;
        for(int i = 0; i < Objects.Count; i++)
        {
            Gizmos.DrawSphere(Objects[i].transform.position, 0.5f);
        }
    }
}
