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
    [SerializeField] Scr_PlayerLook Look;
    public Transform SpawnPos;
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
    public string sceneToLoad;
    public int currentSceneIndex;

    bool IsTransitioning = false;
    
    private void OnLevelWasLoaded(int level)
    {
      
    }
    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
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
    }
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
   /* public void LoadScene(int _sceneIndex)
    {
        if (!IsTransitioning)
        {
            LockTransition();
            sceneToLoad = SceneName;
            FadeDark();
            Invoke("TransitionScene",1);
            Invoke("FadeLight",2);
            Invoke("UnlockTransition",2);
        }
    }*/
    IEnumerator LoadScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!operation.isDone)
        {
            yield return null;
        }
    }

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

    public void FadeDark()
    {
        loadingScreen.SetActive(true);
        StartCoroutine(FadeLoadingScreen(1,0.5f));
    }

    public void FadeLight()
    {

        StartCoroutine(FadeLoadingScreen(0,0.5f));
    }

    public void TransitionScene()
    {
        StartCoroutine(LoadScene());
    }

    public void LockTransition()
    {
        IsTransitioning = true;
    }
    public void UnlockTransition()
    {
        IsTransitioning = false;
    }

    public void Respawn()
    {
        Look.m_YRotation = SpawnPos.rotation.y;
        Look.transform.position = SpawnPos.position;
    }


    public void SpawnCheckpoint()
    {
        if (!IsTransitioning)
        {
            LockTransition();
            FadeDark();
            Invoke("Respawn", 1);
            Invoke("FadeLight", 2);
            Invoke("UnlockTransition", 2);
        }
    }

    public void SetSpawnPoint(Transform RespawnPoint)
    {
        SpawnPos = RespawnPoint;
    }
}
