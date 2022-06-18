using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Script_SceneManager : MonoBehaviour
{
    bool IsTransitioning = false;
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

    public GameObject loadingScreen;
    public string sceneToLoad;
    public CanvasGroup canvasGroup;
    public void LoadScene(string SceneName)
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
    }
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
}
