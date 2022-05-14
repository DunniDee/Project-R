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
    
    public GameObject GetPlayerGUI()
    {
        return GameObject.FindGameObjectWithTag("PlayerGUI");
    }

    private void ProcessInput(){
        if(Input.GetKeyDown(menuKey) && isMenuActive){
            ToggePlayerGUI(true);
        }
        else if(Input.GetKeyDown(menuKey) && !isMenuActive)
        {
            PauseGame();
        }
    }
     private void ToggleCursor(){
        Cursor.visible = !Cursor.visible;
        Cursor.lockState = Cursor.visible ? CursorLockMode.Locked  : CursorLockMode.None;
    }

    public void ToggleMenu(bool _activeState){
        isMenuActive = !isMenuActive;
        
        menuObject.SetActive(isMenuActive);
        GetPlayerGUI().SetActive(!isMenuActive);

        ToggleCursor();
        PauseGame();
    }

    public void ToggePlayerGUI(bool _activeState){

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
