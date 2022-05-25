using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_RoomDoor : MonoBehaviour
{
    [SerializeField] bool m_IsOpen = false;
    [SerializeField] float m_DoorAcceleration = 5;
    Transform m_OpenPos;
    Transform m_ClosePos;
    Transform m_DoorPos;

    // Start is called before the first frame update
    void Start()
    {
        m_OpenPos = transform.GetChild(0);
        m_ClosePos = transform.GetChild(1);
        m_DoorPos = transform.GetChild(2);
    }

    public void SetIsOpen(bool _isOpen)
    {
        m_IsOpen = _isOpen;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsOpen)
        {
            if (m_DoorPos.position != m_OpenPos.position)
            {
                m_DoorPos.position = Vector3.Lerp(m_DoorPos.position,m_OpenPos.position, Time.deltaTime * m_DoorAcceleration);
            }
        }
        else
        {
            if (m_DoorPos.position != m_ClosePos.position)
            {
                m_DoorPos.position = Vector3.Lerp(m_DoorPos.position,m_ClosePos.position, Time.deltaTime * m_DoorAcceleration);
            }
        }
    }
}
