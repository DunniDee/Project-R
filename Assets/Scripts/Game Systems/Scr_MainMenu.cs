using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Scr_MainMenu : MonoBehaviour
{
    [System.Serializable]
    public struct LevelUI_Element {
        public TMP_Text BestRank_TextMesh;
        public TMP_Text BestTime_TextMesh;
    }

    public List<LevelUI_Element> LevelUIElements;

    public string GetMinutesSecondsText(string _LevelName)
    {
        float bestTime = PlayerPrefs.GetFloat(_LevelName+"_bestTime");
        float minutes = Mathf.RoundToInt(bestTime / 60);
        float seconds = Mathf.RoundToInt(bestTime % 60);

        string minuteText = null;
        string secondsText = null;
        if (minutes < 10)
        {
            minuteText = "0" + minutes.ToString();
        }
        if (seconds < 10)
        {
            secondsText = "0" + Mathf.RoundToInt(seconds).ToString();
        }
        string finaltext = (minuteText + ":" + secondsText);
        return finaltext;
    }
    public void InitaliseLevelScores()
    {
        LevelUIElements[0].BestTime_TextMesh.text = GetMinutesSecondsText("Construction 01");
        LevelUIElements[0].BestRank_TextMesh.text = PlayerPrefs.GetString("Construction 01_rank", "C");

        LevelUIElements[1].BestTime_TextMesh.text = GetMinutesSecondsText("Wallrun_02");
        LevelUIElements[1].BestRank_TextMesh.text = PlayerPrefs.GetString("Wallrun_02_rank", "C");

        LevelUIElements[2].BestTime_TextMesh.text = GetMinutesSecondsText("BoilerRoom_03");
        LevelUIElements[2].BestRank_TextMesh.text = PlayerPrefs.GetString("BoilerRoom_03_rank", "C");

    }

    private void Awake()
    {
        InitaliseLevelScores();
        Scr_MenuController.i.SetCursorActive(true);
    }

    private void Start()
    {
    }

    public void Play(string _SceneToLoad)
    {
        SceneManager.LoadScene(_SceneToLoad);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
