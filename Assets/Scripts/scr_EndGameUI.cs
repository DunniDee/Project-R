using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class scr_EndGameUI : MonoBehaviour
{
    public Animator animator;

    public TMP_Text CompletionTime_TextMesh;
    public TMP_Text KillCount_TextMesh;

    public void ShowEndGameUI()
    {
        animator.SetTrigger("Show");
        FindObjectOfType<Script_HitMarker>().gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        scr_GameManager.i.isLevelComplete = true;
        SetUIElements();
    }

    private void SetUIElements()
    {
        CompletionTime_TextMesh.text = scr_GameManager.i.CurrentTimePlayed.ToString("F0");
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
