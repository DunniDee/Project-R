using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Scr_ScreenSpaceHud : MonoBehaviour
{
    [SerializeField] Scr_PlayerHealth Health;
    [SerializeField] Scr_PlayerMotor Motor;
    [SerializeField] int WasCount;

    [SerializeField] Image[] DashImages;

    bool Dash1;
    bool Dash2;
    bool Dash3;

    private void Start()
    {
        Health = GetComponentInParent<Scr_PlayerHealth>();

    }

    public void HealScreenEffect()
    { 
       
    }
    public void DamageScreenEffect()
    {
     
    }
    // Update is called once per frame
    void Update()
    {
       /* if (BloodyScreenImage.color.a > 0)
        {
            BloodyScreenImage.color = Color.Lerp(BloodyScreenImage.color, new Color(BloodyScreenImage.color.r, BloodyScreenImage.color.g, BloodyScreenImage.color.b, 0), Time.deltaTime * 10);
        }*/
        if (Motor.m_DashCount != WasCount)
        {
            switch (Motor.m_DashCount)
            {
                case 0:
                    Dash1 = false;
                    Dash2 = false;
                    Dash3 = false;
                break;
                
                case 1:
                    Dash1 = true;
                    Dash2 = false;
                    Dash3 = false;
                break;

                case 2:
                    Dash1 = true;
                    Dash2 = true;
                    Dash3 = false;
                break;

                case 3:
                    Dash1 = true;
                    Dash2 = true;
                    Dash3 = true;
                break;  

                default:
                break;
            }
        }

        
        if (Dash1)
        {
            DashImages[0].color = Color.Lerp(DashImages[0].color, new Color(1,1,1,1), Time.deltaTime * 10);
        }
        else
        {
            DashImages[0].color = Color.Lerp(DashImages[0].color, new Color(0,0,0,0), Time.deltaTime * 10);
        }

        if (Dash2)
        {
            DashImages[1].color = Color.Lerp(DashImages[1].color, new Color(1,1,1,1), Time.deltaTime * 10);
        }
        else
        {
            DashImages[1].color = Color.Lerp(DashImages[1].color, new Color(0,0,0,0), Time.deltaTime * 10);
        }

        if (Dash3)
        {
            DashImages[2].color = Color.Lerp(DashImages[2].color, new Color(1,1,1,1), Time.deltaTime * 10);
        }
        else
        {
            DashImages[2].color = Color.Lerp(DashImages[2].color, new Color(0,0,0,0), Time.deltaTime * 10);
        }

        WasCount = Motor.m_DashCount;
    }

    
}
