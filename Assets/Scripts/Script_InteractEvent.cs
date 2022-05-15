﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//[System.Serializable]
public class Script_InteractEvent : MonoBehaviour
{
    public enum InteractEventType
    {
        Interact,
        ColliderEnter,
        ColliderExit,
    }

    private BoxCollider m_TriggerCollider;
    
    //Custom Editor Properties
    [HideInInspector]
    public  InteractEventType EventType;


    [HideInInspector]
    public UnityEvent InteractEvent;

    [HideInInspector]
    public Vector3 TriggerCenter;
    [HideInInspector]
    public Vector3 TriggerSize = Vector3.one;
        [HideInInspector]
    public Color TriggerColor = Color.green;
    [HideInInspector]
    public bool DoEventOnce = false;
    [HideInInspector]
    public bool IsActive = true;

    private bool hasEventTrigger = false;

    public void Interact()
    {
        if(IsActive)
        {
             if(DoEventOnce)
            {
                if(!hasEventTrigger && InteractEvent != null)
                {
                    InteractEvent.Invoke();
                }
                hasEventTrigger = true;
            }
            else
            {
                if(InteractEvent != null)
                {
                    InteractEvent.Invoke();
                }
            }
        }
       
    }

    public void SetEventActive(bool _bool)
    {
        IsActive = _bool;
    }

    void CheckEventType()
    {
        switch(EventType)
        {
            case InteractEventType.Interact:
                if(m_TriggerCollider != null)
                    Destroy(m_TriggerCollider);
                break;
            case InteractEventType.ColliderEnter:
                if(m_TriggerCollider == null)
                {
                    m_TriggerCollider = gameObject.AddComponent<BoxCollider>();
                    m_TriggerCollider.isTrigger = true;
                    m_TriggerCollider.center = TriggerCenter;
                    m_TriggerCollider.size = TriggerSize;
                }
                break;
            case InteractEventType.ColliderExit:
                if(m_TriggerCollider == null)
                {
                    m_TriggerCollider = gameObject.AddComponent<BoxCollider>();
                    m_TriggerCollider.isTrigger = true;
                    m_TriggerCollider.center = TriggerCenter;
                    m_TriggerCollider.size = TriggerSize;
                }
                break;
        }
        
    }

    void Start()
    {
        CheckEventType();
    }

    void OnValidate()
    {
        gameObject.layer = LayerMask.NameToLayer("Interactable");      
    }

    void OnTriggerEnter()
    {
        if(EventType == InteractEventType.ColliderEnter)
        {
            Interact();
        }
    }

    void OnTriggerExit()
    {
        if(EventType == InteractEventType.ColliderExit)
        {
            Interact();
        }
    }

    async void OnDrawGizmos()
    {
        if(EventType == InteractEventType.ColliderEnter || EventType == InteractEventType.ColliderExit)
        {
           Gizmos.color = TriggerColor;
           Gizmos.DrawCube(transform.position + TriggerCenter, TriggerSize);
        }

    }
}
