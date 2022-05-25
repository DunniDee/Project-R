using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerInteract : MonoBehaviour
{
    [Header("Internal Components")]
    public Transform MainCamera;

    [Header("Interact Properties")]
    public float m_InteractDistance = 5f;
    public LayerMask m_InteractLayer;
    void Interact()
    {
        RaycastHit hit;
        if (Physics.Raycast(MainCamera.position, MainCamera.forward, out hit, m_InteractDistance, m_InteractLayer))
        {
            if (hit.collider.GetComponent<Script_InteractEvent>() != null)
            {
                Debug.Log("Interacting with " + hit.collider.name);
                hit.collider.GetComponent<Script_InteractEvent>().Interact();
            }
            else
            {
                Debug.Log("No Interact Script Attached to " + hit.collider.name);
            }
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(MainCamera.position, MainCamera.forward * m_InteractDistance);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }
}
