using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class scr_EndGameUI : MonoBehaviour
{
    public Animator animator;

    [Header("EndGameUI Components")]
    //Buttons to Assign OnClick
    public Button NextLevel_Button;
    public Button Retry_Button;
    public Button MainMenu_Button;

    [SerializeField] AudioSource m_audioSource;
    [SerializeField] AudioClip m_TickAudio;
    [SerializeField] AudioClip m_TickFinish;

    [Header("UI Components")]
    public TMP_Text CompletionTime_TextMesh;
    public TMP_Text ScorePrompt_TextMesh;
    public TMP_Text KillCount_TextMesh;
    public TMP_Text OverRank_TextMesh;

    public GameObject NewBestTime_TextObject; // Used to hide unless is new best time.


    public List<string> ScorePrompts; // Score Prompts order from 0 - 3 (Lowest score to Heighest score)

    private bool isNewBest = false;

    // TURN OFF WHEN BUILDING THE GAME!!! - Added by Ash
    public bool DebugMode = true;

    public void ShowEndGameUI()
    {
        //Show Endgame UI Canvas Animation
        animator.SetTrigger("Show");

        SetUIElements();
        
        //Activate Cursor And Pause Player Motor Controls
        Scr_MenuController.i.SetCursorActive(true);
        
        //Set GameState to Complete
        scr_GameManager.i.isLevelComplete = true;
    }

    /// <summary>
    /// Save the bestTimePlayed for this Scene
    /// </summary>
    public void SavePlayerBestTime(char _bestTime)
    {
        //If the current time played is less than the previous best time. save the current time played.
        if (scr_GameManager.i.CurrentTimePlayed < PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "_bestTime", 0))
        {
            PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "_bestTime", scr_GameManager.i.CurrentTimePlayed);
            PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "_rank", _bestTime.ToString());
            NewBestTime_TextObject.SetActive(true);
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
        for (int i = 0; i < scr_GameManager.i.CurrentTimePlayed; i++)
        {
            completionTime++;
            
            CompletionTime_TextMesh.text = GetMinutesSecondsText(completionTime);
            m_audioSource.pitch += 0.1f;
            m_audioSource.PlayOneShot(m_TickAudio);
            yield return new WaitForSeconds(0.01f);
        }
        m_audioSource.pitch = 1;
        m_audioSource.PlayOneShot(m_TickFinish);
       


        //Display Number of Cultists Killed
        for (int i = 0; i < scr_GameManager.i.EnemyKillCount; i++)
        {
            completionTime++;

            KillCount_TextMesh.text = i.ToString();
            m_audioSource.pitch += 0.1f;
            m_audioSource.PlayOneShot(m_TickAudio);
            yield return new WaitForSeconds(0.01f);
        }
        m_audioSource.pitch = 1;
        m_audioSource.PlayOneShot(m_TickFinish);

        //Display Map Completetion Rank
        DisplayCompleteRank();

        yield return null;
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
        if (OverRank_TextMesh.gameObject.activeSelf == false)
        {
            OverRank_TextMesh.gameObject.SetActive(true);
            ScorePrompt_TextMesh.gameObject.SetActive(true);
        }

        char CompletetionRank = scr_GameManager.i.GetLevelCompletionRank();

        if (CompletetionRank == 'C') // C rank
        {
            OverRank_TextMesh.text = "C";
            ScorePrompt_TextMesh.text = ScorePrompts[0];
           

        }
        else if (CompletetionRank == 'B') // B rank
        {
            OverRank_TextMesh.text = "B";
            ScorePrompt_TextMesh.text = ScorePrompts[1];
         
        }
        else if (CompletetionRank == 'A') // A rank
        {
            OverRank_TextMesh.text = "A";
            ScorePrompt_TextMesh.text = ScorePrompts[2];
           

        }
        else if (CompletetionRank == 'S') // S rank
        {
            OverRank_TextMesh.text = "S";
            ScorePrompt_TextMesh.text = ScorePrompts[3];
          
        }
        SavePlayerBestTime(CompletetionRank);
    }

    /// <summary>
    /// Called before the FirstFrame
    /// </summary>
    private void Start()
    {
        animator = GetComponent<Animator>();

        Retry_Button.onClick.AddListener(delegate { Script_SceneManager.Instance.LoadCurrentScene();} );
        NextLevel_Button.onClick.AddListener(delegate { Script_SceneManager.Instance.LoadNextScene(); });
        MainMenu_Button.onClick.AddListener(delegate { Script_SceneManager.Instance.LoadScene("MenuLevelScene"); });
    }

    //Called Everyframe
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0) && DebugMode == true)
        {
            ShowEndGameUI();
        }
    }
}
