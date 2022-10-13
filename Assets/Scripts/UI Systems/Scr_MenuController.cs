/*****************************************************************/
/* NAME: Ashley Rickit */
/* ORGN: Greeble */
/* FILE: GridController.cs */
/* DATE:  */
/*****************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Scr_MenuController: MonoBehaviour
{
    public static Scr_MenuController i;

    [Header("Key Binds")]
    public KeyCode PauseMenuButton = KeyCode.Tab;
    [Header("Internal Components")]
    public RectTransform m_MenuRectTransform;
    public Transform m_MenuCanvasTransform;
    public AudioSource audioSource;
    public bool isMenuActive = false;

    [Header("Audio Assets")]
    public AudioClip OnMouseClickAudio;
    public AudioClip OnMouseRollOverAudio;

    public Transform GetUICanvas()
    {
        return m_MenuCanvasTransform;
    }

    public void PlayRollOverAudio()
    {
        audioSource.PlayOneShot(OnMouseRollOverAudio);
    }

    public void PlayClickAudio()
    {
        audioSource.PlayOneShot(OnMouseClickAudio);
    }

    #region Getters & Setters
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_Fullscreen"></param>
    public void SetFullScreen(bool _Fullscreen)
    {
        Screen.fullScreen = _Fullscreen;
    }

    /// <summary>
    /// /
    /// </summary>
    /// <param name="_graphicsIndex"></param>
    public void SetQuality(int _graphicsIndex)
    {
        QualitySettings.SetQualityLevel(_graphicsIndex);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_Audio"></param>
    public void SetMasterAudio(float _Audio)
    {
        AudioListener.volume = _Audio;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_b"></param>
    public void SetUIActive(bool _b)
    {
        m_MenuCanvasTransform.gameObject.SetActive(_b);
        SetCursorActive(_b);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_b"></param>
    public void SetCursorActive(bool _b)
    {
        FindObjectOfType<Scr_PlayerMotor>().enabled = !_b;
        FindObjectOfType<Scr_PlayerLook>().enabled = !_b;
        FindObjectOfType<Scr_PlayerHealth>().enabled = !_b;
        script_WeaponSwap.Instance.SetCanShoot(!_b);

        Cursor.visible = _b;
        Cursor.lockState = Cursor.visible ? CursorLockMode.Confined : CursorLockMode.Locked;
    }

  

    #endregion


   
   
   


    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(PauseMenuButton))
        {
            if (!isMenuActive)
            {
                SetUIActive(true);
            }
            else {
                SetUIActive(false);
            }
        }
    }
}
