using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Script_SceneManager : MonoBehaviour
{
    [SerializeField] GameObject LoaderCanvas;
    [SerializeField] GameObject PressKeyToContinue;
    public static Script_SceneManager Instance; // Singleton
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async void LoadScene(string sceneName)
    {
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        LoaderCanvas.SetActive(true);
        PressKeyToContinue.SetActive(false);

        do
        {
            
        } while (scene.progress < 0.9f);

        PressKeyToContinue.SetActive(true);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoaderCanvas.SetActive(false);
            PressKeyToContinue.SetActive(false);
            scene.allowSceneActivation = true;
        }
    }

    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            LoadScene("TestLevel");
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            LoadScene("HubWorld");
        }
    }

}
