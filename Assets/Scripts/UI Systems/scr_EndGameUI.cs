using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class scr_EndGameUI : MonoBehaviour
{
    public Animator animator;

    public TMP_Text CompletionTime_TextMesh;
    public TMP_Text KillCount_TextMesh;
    public TMP_Text OverRank_TextMesh;

    public GameObject NewBestTime_TextObject; // Used to hide unless is new best time.
    public GameObject EndGameCanvasObject;

    private bool isNewBest = false;



    // TURN OFF WHEN BUILDING THE GAME!!! - Added by Ash
    public bool DebugMode = true;

    public void ShowEndGameUI()
    {
        //Enable EndGame Canvas
        EndGameCanvasObject.SetActive(true);

        //Show Endgame UI Canvas Animation
        animator.SetTrigger("Show");

        //Hide Hitmarker Gameobject (fixed some bug with UI canvas raycasting -Ash)
        //FindObjectOfType<Script_HitMarker>().gameObject.SetActive(false);
        
        
        //Activate Cursor And Pause Player Motor Controls
        Scr_MenuController.i.SetCursorActive(true);
        
        //Set GameState to Complete
        scr_GameManager.i.isLevelComplete = true;
        SetUIElements();
    }

    /// <summary>
    /// Save the bestTimePlayed for this Scene
    /// </summary>
    public void SetPlayerPrefs()
    {
        if (PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "_bestTime", 0) < scr_GameManager.i.CurrentTimePlayed)
        {
            PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "_bestTime", scr_GameManager.i.CurrentTimePlayed);
        }
       
    }
    public string GetMinutesSecondsText(float FinishTime)
    {
        float minutes = Mathf.RoundToInt(FinishTime / 60);
        float seconds = Mathf.RoundToInt(FinishTime % 60);

        string minuteText = null;
        string secondsText = null;
        if (minutes < 10)
        {
            minuteText = "0" + minutes.ToString();
        }
        else
        {
            minuteText = Mathf.RoundToInt(minutes).ToString();
        }

        if (seconds < 10)
        {
            secondsText = "0" + Mathf.RoundToInt(seconds).ToString();
        }
        else
        { 
            secondsText = Mathf.RoundToInt(seconds).ToString();
        }
        string finaltext = (minuteText + ":" + secondsText);
        return finaltext;
    }

    /// <summary>
    /// Displays the UI Icons in sequence
    /// </summary>
    /// <returns></returns>
    private IEnumerator DisplayCompletionScoreCoroutine()
    {
        float completionTime = 0.0f;
        for (int i = 0; i < scr_GameManager.i.CurrentTimePlayed; i++)
        {
            completionTime += 1;
            yield return new WaitForSeconds(0.01f/i);
            CompletionTime_TextMesh.text = GetMinutesSecondsText(completionTime);
        }
       
        yield return new WaitUntil(() => completionTime == scr_GameManager.i.CurrentTimePlayed);
        //Display Best Score

        //Display Medal / Letter Ranking


    }
    /// <summary>
    /// 
    /// </summary>
    private void SetUIElements()
    {
        //Get Completetion Rank from GameManager
        char CompletetionRank = scr_GameManager.i.GetLevelCompletionRank();

        //Set Completetion Time
        StartCoroutine(DisplayCompletionScoreCoroutine());

        if (CompletetionRank == 'C') // C rank
        {
            OverRank_TextMesh.text = "C";
            PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "_rank", "C");
        }
        else if (CompletetionRank == 'B') // B rank
        {
            OverRank_TextMesh.text = "B";
            PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "_rank", "B");
        }
        else if (CompletetionRank == 'A') // A rank
        {
            OverRank_TextMesh.text = "A";
            PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "_rank", "A");

        }
        else if (CompletetionRank == 'S') // S rank
        {
            OverRank_TextMesh.text = "S";
            PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "_rank", "S");
        }

        KillCount_TextMesh.text = scr_GameManager.i.EnemyKillCount.ToString("F0");
    }
        
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad7) && DebugMode == true)
        {
            ShowEndGameUI();
        }
    }
}
