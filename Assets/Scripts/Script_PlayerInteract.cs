using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Script_PlayerInteract : MonoBehaviour
{
    [Header("Internal Components")]
    public Transform MainCamera;
    public GameObject InteractText;

    [Header("Interact Properties")]
    public KeyCode InteractKey = KeyCode.F;
    public float InteractRayLength = 5f;
    public LayerMask InteractLayer;

    //[SerializeField] Script_InteractEvent hitEvent;

    //Interact function casts a ray looking for a interactable layer.
    void Interact()
    {
        RaycastHit hit;
        if (Physics.Raycast(MainCamera.position, MainCamera.forward, out hit, InteractRayLength, InteractLayer))
        {
            // if the Interact event component is attached to the object play the interact event.
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

    void HandleInteractText()
    {
        RaycastHit hit;
        if (Physics.Raycast(MainCamera.position, MainCamera.forward, out hit, InteractRayLength, InteractLayer))
        {
            if (hit.transform.GetComponent<Script_InteractEvent>().GetHasEventTriggered() && InteractText.activeSelf)
            {
                InteractText.SetActive(false);
            }
            else if(!InteractText.activeSelf && !hit.transform.GetComponent<Script_InteractEvent>().GetHasEventTriggered())
            {
                InteractText.SetActive(true);
            }

        }
        else if (InteractText.activeSelf)
        {
            InteractText.SetActive(false);
        }
    }
    // Gizmos draws a line in front of the player
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(MainCamera.position, MainCamera.forward * InteractRayLength);
    }

    // Update called everyframe
    void Update()
    {
        if (Input.GetKeyDown(InteractKey))
        {
            Interact();
        }
        HandleInteractText();
    }
}
