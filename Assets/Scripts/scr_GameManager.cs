using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class scr_GameManager : MonoBehaviour
{
    public static scr_GameManager i;

    [Header("Internal Properties")]
    public GameObject TotalScore_Gameobject;
    public TMP_Text TotalScore_TextMesh;

    public bool isLevelComplete = false;

    [Header("Level Properties")]
    public float CurrentTimePlayed = 0;
    public List<float> TimesToBeat;

    public int EnemyKillCount = 0;
    
    public float TotalScore = 0.0f;

    private float scoreUpdateTimer = 0.0f;
    private float scoreUpdateTime = 1.5f;
    private bool isUpdatingScore = false;
  
   // public void IncreaseScore(float _ScoreAmount);
    public void IncreaseTotalScore(float _ScoreAmount)
    {
        TotalScore += _ScoreAmount;
        scoreUpdateTimer = scoreUpdateTime;
        isUpdatingScore = true;
    }
    private void UpdateScoreUIText()
    {
        TotalScore_TextMesh.text = TotalScore.ToString();
        isUpdatingScore = false;
    }
    public void IncreaseKillCount()
    {
        EnemyKillCount++;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        if (i == null)
        {
            i = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (isLevelComplete == false)
        {
            CurrentTimePlayed += Time.deltaTime;
        }
        if (scoreUpdateTime > 0.0f && isUpdatingScore == true)
        {
            scoreUpdateTime -= Time.deltaTime;
        }
        else if (scoreUpdateTime <= 0.0f && isUpdatingScore == true)
        {
            UpdateScoreUIText();
        }

        
    }
}
