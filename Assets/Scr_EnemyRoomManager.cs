using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Scr_EnemyRoomManager : MonoBehaviour
{
    [SerializeField] UnityEvent OnExtermination;
    [SerializeField] List<Scr_BaseAI> EnemyList;

    [SerializeField] List<Scr_BaseAI> Wave1;
    int m_Wave1Count;
    [SerializeField] List<Scr_BaseAI> Wave2;
    int m_Wave2Count;
    [SerializeField] List<Scr_BaseAI> Wave3;

    bool IsClear;
    bool WasClear;

    int m_MaxCount;

    private void Start() 
    {
        foreach (var item in Wave1)
        {
            EnemyList.Add(item);
        }

        foreach (var item in Wave2)
        {
            EnemyList.Add(item);
        }

        foreach (var item in Wave3)
        {
            EnemyList.Add(item);
        }

        m_MaxCount = EnemyList.Count;
        m_Wave1Count = Wave1.Count;
        m_Wave2Count = Wave2.Count;

    }

    void Update()
    {
        //check the lkist of enemies
        foreach (var Enemy in EnemyList)
        {
            if (Enemy == null) //clear any dead enemies
            {
                EnemyList.Remove(Enemy);
            }
        }

        if (EnemyList.Count == 0)
        {
            IsClear = true;
        }
        else
        {
            IsClear = false;
        }

        if (IsClear && !WasClear)
        {
            OnExtermination.Invoke();
        }

        CheckWave();
    }


    int lastCount;
    void CheckWave()
    {
        if (EnemyList.Count != lastCount)
        {
            if (EnemyList.Count <= m_MaxCount - (m_Wave1Count + m_Wave2Count))
            {
                foreach (var Enemy in Wave3)
                {
                    if (Enemy == null) //clear any dead enemies
                    {
                        Wave3.Remove(Enemy);
                    }
                }

                foreach (var Enemy in Wave3)
                {
                    Enemy.gameObject.SetActive(true);
                }
            }

            if (EnemyList.Count <= m_MaxCount - m_Wave1Count)
            {
                foreach (var Enemy in Wave2)
                {
                    if (Enemy == null) //clear any dead enemies
                    {
                        Wave2.Remove(Enemy);
                    }
                }

                foreach (var Enemy in Wave2)
                {
                    Enemy.gameObject.SetActive(true);
                }
            }

        }

        lastCount = EnemyList.Count;
    }
}
