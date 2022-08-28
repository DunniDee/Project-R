using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class scr_BoilerRoom : MonoBehaviour
{

    public UnityEvent OnRoomCompletedEvent;
    [SerializeField] int ButtonsPressed = 0;

    public void IncrementButton()
    {
        ButtonsPressed++;
        if (ButtonsPressed >= 3)
        {
            //Do Thing
            if (OnRoomCompletedEvent != null)
            {
                OnRoomCompletedEvent.Invoke();

            }
          
        }
        
    }
   
}
