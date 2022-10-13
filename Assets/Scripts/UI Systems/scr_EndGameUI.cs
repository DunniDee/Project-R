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



    public void ShowEndGameUI()
    {
        animator.SetTrigger("Show");
        FindObjectOfType<Script_HitMarker>().gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        scr_GameManager.i.isLevelComplete = true;
        SetUIElements();
    }

    public void SetPlayerPrefs()
    {
        if (PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "_bestTime", 0) < scr_GameManager.i.CurrentTimePlayed)
        {
            PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "_bestTime", scr_GameManager.i.CurrentTimePlayed);
        }
       
    }
    private void SetUIElements()
    {
        CompletionTime_TextMesh.text = scr_GameManager.i.CurrentTimePlayed.ToString("F0");

        if(scr_GameManager.i.CurrentTimePlayed > scr_GameManager.i.TimesToBeat[3]) // C rank
        {
            OverRank_TextMesh.text = "C";
            PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "_rank", "C");
        }
        else if (scr_GameManager.i.CurrentTimePlayed < scr_GameManager.i.TimesToBeat[2]) // B rank
        {
            OverRank_TextMesh.text = "B";
            PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "_rank", "B");
        }
        else if (scr_GameManager.i.CurrentTimePlayed < scr_GameManager.i.TimesToBeat[1]) // A rank
        {
            OverRank_TextMesh.text = "A";
            PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "_rank", "A");

        }
        else if (scr_GameManager.i.CurrentTimePlayed < scr_GameManager.i.TimesToBeat[0]) // S rank
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
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            ShowEndGameUI();
        }
    }
}
