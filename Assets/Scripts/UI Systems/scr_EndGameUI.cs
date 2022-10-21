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

        SetUIElements();
        //Hide Hitmarker Gameobject (fixed some bug with UI canvas raycasting -Ash)
        //FindObjectOfType<Script_HitMarker>().gameObject.SetActive(false);
        
        
        //Activate Cursor And Pause Player Motor Controls
        Scr_MenuController.i.SetCursorActive(true);
        
        //Set GameState to Complete
        scr_GameManager.i.isLevelComplete = true;
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
    /// Displays the UI Time completed score and medal / overall rank.  in sequence
    /// </summary>
    /// <returns></returns>
    private IEnumerator DisplayCompletionScoreCoroutine()
    {
        float completionTime = 0.0f;
        CompletionTime_TextMesh.gameObject.GetComponentInParent<GameObject>().SetActive(true);
        for (int i = 0; i < scr_GameManager.i.CurrentTimePlayed; i++)
        {
            completionTime++;
            
            CompletionTime_TextMesh.text = GetMinutesSecondsText(completionTime);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitUntil(() => completionTime == scr_GameManager.i.CurrentTimePlayed);


        //Display Medal / Letter Ranking

        if (OverRank_TextMesh.gameObject.GetComponentInParent<GameObject>().activeSelf == false)
        {
            //enable parent of overall rank textmesh 
            OverRank_TextMesh.gameObject.GetComponentInParent<GameObject>().SetActive(true);
        }

        DisplayCompleteRank();

        KillCount_TextMesh.text = scr_GameManager.i.EnemyKillCount.ToString("F0");
    }

    /// <summary>
    /// Called when the Animator is played
    /// </summary>
    public void SetUIElements()
    {
        //Set Completetion Time
        StartCoroutine(DisplayCompletionScoreCoroutine());

        
    }

    private void DisplayCompleteRank()
    {
        char CompletetionRank = scr_GameManager.i.GetLevelCompletionRank();

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
