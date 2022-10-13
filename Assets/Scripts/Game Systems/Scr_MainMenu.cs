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

    public void InitaliseLevelScores()
    {
        LevelUIElements[0].BestTime_TextMesh.text = PlayerPrefs.GetFloat("Construction_01_bestTime", 0).ToString();
        LevelUIElements[0].BestRank_TextMesh.text = PlayerPrefs.GetString("Construction_01_rank", null).ToString();

        LevelUIElements[1].BestTime_TextMesh.text = PlayerPrefs.GetFloat("Wallrun_02_bestTime", 0).ToString();
        LevelUIElements[1].BestRank_TextMesh.text = PlayerPrefs.GetString("Wallrun_02_rank", null).ToString();

        LevelUIElements[2].BestTime_TextMesh.text = PlayerPrefs.GetFloat("BoilerRoom_03_bestTime", 0).ToString();
        LevelUIElements[2].BestRank_TextMesh.text = PlayerPrefs.GetString("BoilerRoom_03_rank", null).ToString();

    }
    private void Awake()
    {
        InitaliseLevelScores();
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
