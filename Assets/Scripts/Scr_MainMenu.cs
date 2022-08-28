using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scr_MainMenu : MonoBehaviour
{
    public void Play(string _SceneToLoad)
    {
        SceneManager.LoadScene(_SceneToLoad);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
