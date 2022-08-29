using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Script_LevelTrigger : MonoBehaviour
{
    public bool ReloadCurrentScene = false;
    public void LoadScene(string SceneToLoad)
    {
        if (ReloadCurrentScene)
        {
            Script_SceneManager.Instance.LoadScene(SceneManager.GetActiveScene().name);
        }
        else {
            Script_SceneManager.Instance.LoadScene(SceneToLoad);
        }
       
    }

   /* public void LoadScene(int _SceneIndex)
    {
        if (ReloadCurrentScene)
        {
            Script_SceneManager.Instance.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            Script_SceneManager.Instance.LoadScene(SceneToLoad);
        }
    }*/
}
