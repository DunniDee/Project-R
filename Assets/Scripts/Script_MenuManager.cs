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
    public GameObject PlayerUI;

    public TMP_Text Credits;
    public TMP_Text Bounty;
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
     private void ToggleCursor(){
        Cursor.visible = !Cursor.visible;
        Cursor.lockState = Cursor.visible ? CursorLockMode.Confined  : CursorLockMode.Locked;
    }

    public void ToggleMenu(bool _activeState){
        menuObject.SetActive(_activeState);
        isMenuActive = menuObject.active;
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
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayerUI = GetPlayerGUI();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    // Update is called once per frame
    void Update()
    {
        ProcessInput();
        if (isMenuActive)
        {
            Bounty.text = (Script_PlayerStatManager.Instance.Bounty * 100).ToString();
            Credits.text = Script_PlayerStatManager.Instance.Credits.ToString("F0");
        }
    }
}
