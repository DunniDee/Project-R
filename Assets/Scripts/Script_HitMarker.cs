using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Script_HitMarker : MonoBehaviour
{
    [SerializeField] AudioSource AS;
    [SerializeField] AudioClip HitSound;
    [SerializeField] AudioClip CritSound;
    [SerializeField] AudioClip KillSound;
 public static Script_HitMarker current;
 bool IsCritical;
 [SerializeField] RectTransform rect;
 [SerializeField] Image KillImage;
 private void Start() 
 {
    //rect = gameObject.GetComponent<RectTransform>();
    //  AS = gameObject.GetComponent<AudioSource>();
    current = this;
 }
    float Timer = 1;
    float KillTimer = 1;
    [SerializeField] Image[] Cross;
    // Update is called once per frame
    void Update()
    {
        if (Timer > 0)
        {
            Timer -= Time.deltaTime;

            for (int i = 0; i < 4; i++)
            {
                if (IsCritical)
                {
                    Cross[i].color = new Color(1,0,0,Timer);
                }
                else
                {
                    Cross[i].color = new Color(1,1,1,Timer);
                }
            }

            float Size = Timer * 20;
            rect.sizeDelta = new Vector2(Size,Size);
        }

        if (KillTimer > 0)
        {
            KillTimer -= Time.deltaTime;
            // if (IsCritical)
            // {
            //     KillImage.color = new Color(1,0,0,KillTimer);
            // }
            // else
            // {
            //     KillImage.color = new Color(1,1,1,KillTimer);
            // }
            KillImage.color = new Color(1,0,0,KillTimer);
        }
    }

    public event Action onHit;
    public void HitMarker()
    {
        if (onHit != null)
        {
            onHit();
        }

        AS.PlayOneShot(HitSound);
        IsCritical = false;
        Timer = 1;
    }

    public event Action onCritHit;
    public void CritMarker()
    {
        if (onCritHit != null)
        {
            onCritHit();
        }

        AS.PlayOneShot(CritSound);
        IsCritical = true;
        Timer = 1;
    }

    public event Action onKillHit;
    public void KillMarker()
    {
        if (onKillHit != null)
        {
            onKillHit();
        }

        AS.PlayOneShot(KillSound);
        IsCritical = true;
        Timer = 1;
        KillTimer = 0.5f;
    }

}
