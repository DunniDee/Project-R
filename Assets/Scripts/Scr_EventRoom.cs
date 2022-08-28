using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Scr_EventRoom : MonoBehaviour
{
    [SerializeField] protected Scr_RoomDoor Entrance;
    [SerializeField] protected Scr_RoomDoor Exit;
    public virtual void SetIsOpen(bool _isOpen)
    {
        Exit.SetIsOpen(_isOpen);
    }
    protected void OnTriggerEnter(Collider other) 
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Player"))
        {
            Entrance.SetIsOpen(false);
        }
    }
}   
