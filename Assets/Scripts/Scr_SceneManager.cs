using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scr_SceneManager : MonoBehaviour
{
    public int curSceneIndex = 0;

    public void LoadScene(int _sceneIndex)
    {
        SceneManager.LoadScene(_sceneIndex);
    }

    void OnValidate()
    {
        curSceneIndex = SceneManager.GetActiveScene().buildIndex;

    }

    void SetObjectDelegates(Scr_EnemyShip[] _objects)
    {
        foreach(Scr_EnemyShip obj in _objects)
        {
            obj.onBoardShipEvent += LoadScene;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        SetObjectDelegates(GameObject.FindObjectsOfType<Scr_EnemyShip>());
    }

    void Awake()
    {
        switch(curSceneIndex)
        {
            case 0:
                break;
            case 1:
                break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
