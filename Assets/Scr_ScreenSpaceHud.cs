using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Scr_ScreenSpaceHud : MonoBehaviour
{
    [SerializeField] Scr_PlayerMotor Motor;
    [SerializeField] int WasCount;

    [SerializeField] Image[] DashImages;

    // Update is called once per frame
    void Update()
    {
        if (Motor.m_DashCount != WasCount)
        {
            for (int i = 0; i < 3; i++)
            {
                DashImages[i].enabled = false;
            }

            for (int i = 0; i < Motor.m_DashCount; i++)
            {
                DashImages[i].enabled = true;
            }
        }
        WasCount = Motor.m_DashCount;
    }
}
