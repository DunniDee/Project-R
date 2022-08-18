using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_EndGameUI : MonoBehaviour
{

    public void ToggleCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true; 
    }
    public void Update()
    {
        
    }
    public void Start()
    {
        
    }
}
