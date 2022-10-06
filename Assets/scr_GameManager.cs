using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scr_GameManager : MonoBehaviour
{
    public static scr_GameManager i;

    public int EnemyKillCount = 0;
    public float CurrentTimePlayed = 0;

    public bool isLevelComplete = false;

    public List<float> TimesToBeat;

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
            

        
    }
}
