using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Script_MenuManager : MonoBehaviour
{
    public static Script_MenuManager Instance;

    [Header("Input Config")]
    public KeyCode menuKey = KeyCode.Escape;

    private bool isMenuActive = false;
    public GameObject menuObject;

    public GameObject GetPlayerGUI()
    {
        return GameObject.FindGameObjectWithTag("PlayerGUI");
    }

    private void ProcessInput(){
        if(Input.GetKeyDown(menuKey) && isMenuActive){
            ToggleMenu(false);
        }
        else if(Input.GetKeyDown(menuKey) && !isMenuActive)
        {
            ToggleMenu(true);
        }
    }

    public void ToggleMenu(bool _activeState){
        menuObject.SetActive(_activeState);
        isMenuActive = menuObject.activeSelf;

        LoadoutController.i.SetCursorActive(_activeState);

        if (_activeState)
        {
            Time.timeScale = 0.0f;
        }
        else {
            Time.timeScale = 1.0f;
        }
    }

    public void ToggleOptions(){
        Debug.Log("Menu Toggled");
    }

    public void QuitGame(){
        Debug.Log("Exiting Game...");
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

 
    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }
}
