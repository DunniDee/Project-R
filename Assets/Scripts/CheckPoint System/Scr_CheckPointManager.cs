using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_CheckPointManager : MonoBehaviour
{
    public static Scr_CheckPointManager i;

    [Header("Internal Components")]
    public AudioSource audioSource;
    public AudioClip objectivePopupAudio;

    [Header("CheckPoint Properties")]
    public Transform CurrentCheckPoint;
    public GameObject Player;
    public Scr_PlayerLook PlayerLook;

    [Header("Fade Properties")]
    public CanvasGroup canvas;
    public GameObject FadeObject;

    public bool IsTransitioning { get; private set; }

    private void Awake()
    {
        if (i == null)
        {
            i = this;
        }
        else {
            Destroy(gameObject);
        }
    }
    IEnumerator FadeLoadingScreen(float targetValue, float duration)
    {
        float startValue = canvas.alpha;
        float time = 0;
        while (time < duration)
        {
            canvas.alpha = Mathf.Lerp(startValue, targetValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        canvas.alpha = targetValue;
    }
    public void FadeDark()
    {
        FadeObject.SetActive(true);
        StartCoroutine(FadeLoadingScreen(1, 0.5f));
    }

    public void FadeLight()
    {
        StartCoroutine(FadeLoadingScreen(0, 0.5f));
    }

    public void LockTransition()
    {
        IsTransitioning = true;
    }
    public void UnlockTransition()
    {
        IsTransitioning = false;
    }

    public void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerLook = Player.GetComponent<Scr_PlayerLook>();
        CurrentCheckPoint = Player.transform;
    }

    private void SetPositionOfPlayer()
    {
        Debug.Log("RespawningPlayer");
        Player.transform.position = CurrentCheckPoint.position;
        Scr_PlayerHealth playerHealth = Player.GetComponent<Scr_PlayerHealth>();
        PlayerLook.m_YRotation = CurrentCheckPoint.rotation.eulerAngles.y;
        playerHealth.RespawnPlayer();
        
    }
    public void RespawnPlayer()
    {
        if (!IsTransitioning)
        {
            LockTransition();
            FadeDark();
            Invoke("SetPositionOfPlayer", 1);
            Invoke("FadeLight", 2);
            Invoke("UnlockTransition", 2);
        }
       
    }

    public void SetCurrentCheckPoint(GameObject _Gameobject)
    {
        CurrentCheckPoint = _Gameobject.transform;

        scr_ObjectiveHandler.i.ShowObjective("CheckPoint Reached");
    }
}
