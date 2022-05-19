using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_EnemyShip : MonoBehaviour
{
    [SerializeField]
    private SphereCollider sphereCollider;

    [Header("Detection Radius")]
    [Range(0.0f, 20.0f)]
    [SerializeField] float m_detectionRadius = 2f;

    public delegate void OnShipDetectedDelegate(bool _bool);
    public event OnShipDetectedDelegate onShipDetectedEvent;

    public delegate void OnBoardShipDelegate(int _sceneIndex);
    public event OnBoardShipDelegate onBoardShipEvent;

    public bool isShipInRange = false;
    public void OnTriggerEnter(Collider collider)
    {
        if(onShipDetectedEvent != null && collider.tag == "Player")
        {
            onShipDetectedEvent(true);
            isShipInRange = true;
        }
    }

    public void OnTriggerExit(Collider collider)
    {
        if(onShipDetectedEvent != null)
        {
            onShipDetectedEvent(false);
            isShipInRange = false;
        }
    }

    
    void OnValidate()
    {
        if (sphereCollider == null)
        {
            sphereCollider = GetComponent<SphereCollider>();
        }
    }

    void ProcessInput()
    {
        if(Input.GetKeyDown(KeyCode.E) && isShipInRange)
        {
            //Enter the New Scene
            if(onBoardShipEvent != null)
            {
                Debug.Log("Loading Scene...");
                onBoardShipEvent(1);
            }

        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        sphereCollider.radius = m_detectionRadius;
    }

    // Update is called once per frame
    void Update()
    {
       ProcessInput();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_detectionRadius);
    }
}
