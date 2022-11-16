using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

using TMPro;

public class Scr_SenseSlider : MonoBehaviour
{
    [Header("Internal Components")]
    public Slider m_Slider;
    public TMP_Text SliderText;

    public void SetSlider()
    {
        SliderText.text = "Mouse Sensitivity : " + (m_Slider.value - 25);
    }

    private void Awake()
    {
        m_Slider = GetComponent<Slider>();

        m_Slider.value = PlayerPrefs.GetFloat("MouseSensitivity", 25);
        Debug.Log("Sensitivity Value : " + m_Slider.value);
        SliderText.text = "Mouse Sensitivity : " + (m_Slider.value - 25);
    }
}
