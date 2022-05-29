using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Script_HealthUI : MonoBehaviour
{
    public Slider m_healthSlider;
    public Slider m_followSlider;

    [SerializeField] float m_DecreaseSpeed = 5.0f;

    public void DecreaseValue()
    {
        if(m_followSlider.value != m_healthSlider.value && m_healthSlider.value < m_followSlider.value)
        {
            m_followSlider.value -= m_DecreaseSpeed * Time.deltaTime ;
        }
    }

    public void Update()
    {
        DecreaseValue();
    }
}
