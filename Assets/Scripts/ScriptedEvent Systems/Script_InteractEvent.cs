using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Script_InteractEvent : MonoBehaviour
{
    public enum InteractEventType
    {
        Interact,
        ColliderEnter,
        ColliderExit,
        OnHit,
    }

    [SerializeField] AudioClip m_ActivateClip;
    private AudioSource m_audioSource;
    private ParticleSystem m_particleSystem;
    private BoxCollider m_TriggerCollider;

    #region Editor Properties
    //Custom Editor Properties
    [HideInInspector]
    public InteractEventType EventType;

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
    #endregion

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
                    m_audioSource.PlayOneShot(m_ActivateClip);

                    if (m_particleSystem)
                    {
                        m_particleSystem.Play();
                    }
                }
                hasEventTrigger = true;
            }
            else
            {
                if(InteractEvent != null)
                {
                    InteractEvent.Invoke();
                    m_audioSource.PlayOneShot(m_ActivateClip);
                    
                    if (m_particleSystem)
                    {
                        m_particleSystem.Play();
                    }

                }
            }
        }
       
    }

    public bool GetHasEventTriggered()
    {
        return hasEventTrigger;
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

    //Runs before the first frame
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_particleSystem = GetComponentInChildren<ParticleSystem>();

        CheckEventType();

    }

    #region EventTriggers
    void OnCollisionEnter(Collision collider)
    {
        if (EventType == InteractEventType.OnHit)
        {
            if (collider.gameObject.CompareTag("Projectile"))
            {
                Interact();
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if(EventType == InteractEventType.ColliderEnter)
        {

            if (collider.CompareTag("Player"))
            {
                Interact();
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if(EventType == InteractEventType.ColliderExit)
        {
            if (collider.CompareTag("Player"))
            {
                Interact();
            }
           
        }
    }
    #endregion
    async void OnDrawGizmos()
    {
        if(EventType == InteractEventType.ColliderEnter || EventType == InteractEventType.ColliderExit)
        {
           Gizmos.color = TriggerColor;
           Gizmos.DrawCube(transform.position + TriggerCenter, TriggerSize);
        }

    }
}
