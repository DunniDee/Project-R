using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Script_SceneManager : MonoBehaviour
{

    // Singleton Pattern
    #region Singleton
    public static Script_SceneManager Instance; 
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SceneManager.sceneLoaded += SceneLoaded;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SceneLoaded(Scene _Scene, LoadSceneMode _SceneLoadMode)
    {
        currentSceneIndex = _Scene.buildIndex;
    }
    #endregion

    [Header("Internal Components")]
    public GameObject loadingScreen;
    public CanvasGroup canvasGroup;


    [Header("SceneManager Properties")]
    private string sceneToLoad;
    
    [SerializeField] private int currentSceneIndex;

    bool IsTransitioning = false;

    // MAKE SURE TO TURN OFF WHEN BUILDING THE GAME!!!
    public bool DebugMode = true;
    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1) && DebugMode)
        {
            loadNextSceneInBuildOrder();
        }
    }


    #region DebugMethods
    private void loadNextSceneInBuildOrder()
    {
       
       if (currentSceneIndex < SceneManager.sceneCount)
       {
          currentSceneIndex++;
          LoadScene(SceneManager.GetSceneAt(currentSceneIndex).name);
       }
       else if ((currentSceneIndex + 1) > SceneManager.sceneCount)
       {
           currentSceneIndex = 0;
           LoadScene(SceneManager.GetSceneAt(currentSceneIndex).name);
       }
        
    }
    #endregion

    /// <summary>
    /// load scene methods exectued in ordered seconds for smooth fade in fade out transition.
    /// Scene name passed in as Parameter for level load.
    /// </summary>
    /// <param name="SceneName"></param>
    public void LoadScene(string SceneName)
    {
        if (!IsTransitioning)
        {
            LockTransition();
            sceneToLoad = SceneName;
            FadeDark();
            Invoke("TransitionScene", 1);
            Invoke("FadeLight", 2);
            Invoke("UnlockTransition", 2);
        }
    }

    /// <summary>
    /// load scene methods exectued in ordered seconds for smooth fade in fade out transition.
    /// Scene name passed in as Parameter for level load.
    /// </summary>
    /// <param name="SceneName"></param>
    public void LoadCurrentScene()
    {
        if (!IsTransitioning)
        {
            LockTransition();
            sceneToLoad = SceneManager.GetActiveScene().name;
            FadeDark();
            Invoke("TransitionScene", 1);
            Invoke("FadeLight", 2);
            Invoke("UnlockTransition", 2);
        }
    }
    #region LoadSceneEffects
    /// <summary>
    /// Fade to Full Color - Part of LoadScene
    /// </summary>
    private void FadeDark()
    {
        loadingScreen.SetActive(true);
        StartCoroutine(FadeLoadingScreen(1, 0.5f));
    }

    /// <summary>
    /// Fade to No Color - Part of LoadScene
    /// </summary>
    private void FadeLight()
    {
        StartCoroutine(FadeLoadingScreen(0, 0.5f));
    }

    /// <summary>
    /// Start Transition Scene Corotine - Part of LoadScene
    /// </summary>
    private void TransitionScene()
    {
        StartCoroutine(LoadScene());
    }
    /// <summary>
    /// Set Transistion Bool to True - Part of LoadScene
    /// </summary>
    private void LockTransition()
    {
        IsTransitioning = true;
    }

    /// <summary>
    /// Set transitionting bool to false - Part of LoadScene
    /// </summary>
    private void UnlockTransition()
    {
        IsTransitioning = false;
    }
    #endregion

    /// <summary>
    /// Async Level Loader waits till level load operation is done.
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!operation.isDone)
        {
            yield return null;
        }
    }

    /// <summary>
    /// Fade fadeCanvasGroup to target value over X Duration.
    /// </summary>
    /// <param name="targetValue"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    IEnumerator FadeLoadingScreen(float targetValue, float duration)
    {
        float startValue = canvasGroup.alpha;
        float time = 0;
        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startValue, targetValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = targetValue;
    }
   
}
