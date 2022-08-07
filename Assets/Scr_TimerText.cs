using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scr_TimerText : MonoBehaviour
{
    [SerializeField] TextMeshPro m_Text;
    [SerializeField] Scr_RoomButton m_Button;


    // Update is called once per frame
    void Update()
    {
        m_Text.SetText((Mathf.Clamp(m_Button.m_ToggleTimer,0,999).ToString()).PadLeft(3));
    }
}
