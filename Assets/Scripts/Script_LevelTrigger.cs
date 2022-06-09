using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_LevelTrigger : MonoBehaviour
{
    public void LoadScene(string SceneToLoad)
    {
        Script_SceneManager.Instance.LoadScene(SceneToLoad);
    }
}
