using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

using TMPro;

public class Scr_VolumeSliderUI : MonoBehaviour
{
    [Header("Internal Components")]
    public Slider VolumeSlider;
    public TMP_Text Mainvolume_TextMesh;

    public void SetCurrentMasterVolume()
    {
        AudioListener.volume = VolumeSlider.value;
        Mainvolume_TextMesh.text = "Main Volume : " + (VolumeSlider.value * 100).ToString("F0");
    }

    private void Awake()
    {
        VolumeSlider = GetComponent<Slider>();

        VolumeSlider.value = AudioListener.volume;
        Mainvolume_TextMesh.text = "Main Volume : " + (VolumeSlider.value * 100).ToString("F0");
    }
}
