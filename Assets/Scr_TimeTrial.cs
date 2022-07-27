using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Scr_TimeTrial : MonoBehaviour
{
    public TMP_Text TimeText;
    public TMP_Text curTimeText;
    public TMP_Text finaltext;
    public TMP_Text finalTimeText;

    public float Timer = 0.0f;
    bool isTrialStarted = false;
    bool isFading = true;
    public void StartTimer()
    {
        if (isTrialStarted == false)
        {
            isTrialStarted = true;
            Timer = 0f;
            FadeIn(TimeText, 1.0f);
            FadeIn(curTimeText, 1.0f);
        }
       
    }

    public void StopTimer()
    {
        if (isTrialStarted == true)
        {
            isTrialStarted = false;
        }

    }

    public void FadeIn(TMP_Text _text,float _FadeTime)
    {
        if (isFading)
        {
            _text.CrossFadeAlpha(1.0f, _FadeTime, true);
        }
        
    }

    public void FadeOut(TMP_Text _text, float _FadeTime)
    {
        _text.CrossFadeAlpha(0.0f, _FadeTime, true);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTrialStarted)
        {
            FadeIn(TimeText, 1.0f);
            FadeIn(curTimeText, 1.0f);
            Timer += Time.deltaTime;
            curTimeText.text = Timer.ToString("F2");
        }

    }
}
