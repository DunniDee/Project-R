using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_MenuManager : MonoBehaviour
{
    public static Script_MenuManager Instance;

    [Header("Input Config")]
    public KeyCode menuKey = KeyCode.Escape;

    private bool isMenuActive = false;
    public GameObject menuObject;
    public GameObject PlayerUI;

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
            PauseGame();
        }
    }
     private void ToggleCursor(){
        Cursor.visible = !Cursor.visible;
        Cursor.lockState = Cursor.visible ? CursorLockMode.Locked  : CursorLockMode.None;
    }

    public void ToggleMenu(bool _activeState){
        menuObject.SetActive(_activeState);
        PlayerUI.SetActive(!_activeState);
        FindObjectOfType<Script_PlayerLook>().enabled = !_activeState;

        if (_activeState)
        {
            Time.timeScale = 0.0f;
        }
        else {
            Time.timeScale = 1.0f;
        }

        ToggleCursor();
        PauseGame();
    }

    public void ToggleOptions(){
        Debug.Log("Menu Toggled");
    }

    public void QuitGame(){
        Debug.Log("Exiting Game...");
        Application.Quit();
    }

    void PauseGame()
    {
        FindObjectOfType<Script_PlayerLook>().SetPlayerLookState(!isMenuActive);

    }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        PlayerUI = GetPlayerGUI();
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
