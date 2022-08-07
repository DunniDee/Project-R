using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Scr_RoomButton : MonoBehaviour
{   
    public UnityEvent m_OnToggle;

    public void OnToggle()
    {
        m_OnToggle.Invoke();
    }
    public UnityEvent m_OffToggle;

    public void OffToggle()
    {
        m_OffToggle.Invoke();
    }
    enum ButtonType
    {
        Proximity,
        Press,
        Shoot,
    }

    public bool IsToggled;
    bool WasToggled;
    
    [SerializeField] bool m_IsCountdown;

    bool m_IsCountingDown;
    public float m_ToggleTime;
    public float m_ToggleTimer;

    [SerializeField] ButtonType m_ButtonType;


    // Update is called once per frame
    void Update()
    {
        if (IsToggled && !WasToggled)
        {
            OnToggle();
        }


        if (m_IsCountdown && m_IsCountingDown)
        {
            m_ToggleTimer -= Time.deltaTime;

            if (m_ToggleTimer > 0 )
            {
                IsToggled = true;
                OnToggle();
            }
            else
            {
                IsToggled = false; 
                OffToggle();
                m_IsCountingDown = false;
            }
        }

        WasToggled = IsToggled;
    }

    private void OnTriggerEnter(Collider other) 
    {
        switch (m_ButtonType)
        {
            case ButtonType.Proximity :
                if (other.CompareTag("Player"))
                {
                    if (m_IsCountdown)
                    {
                        m_ToggleTimer = m_ToggleTime;
                    }
                    else
                    {
                        IsToggled = true;
                    }
                }
            break;

            case ButtonType.Press :
            break;
            case ButtonType.Shoot :
                if (other.CompareTag("Projectile"))
                {
                    if (m_IsCountdown)
                    {
                        m_ToggleTimer = m_ToggleTime;
                    }
                    else
                    {
                        IsToggled = true;
                    }
                }
                break;
            default:
            break;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        switch (m_ButtonType)
        {
            case ButtonType.Proximity :
                if (m_IsCountdown)
                {
                    m_IsCountingDown = true;
                }
            break;

            case ButtonType.Press :
            break;
            case ButtonType.Shoot :
                break;
            default:
            break;
        }
    }
}
