using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scr_MainMenu : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip clickNoise;

    public void PlayClickNoise() {
        audioSource.PlayOneShot(clickNoise);
    }

    public void Play()
    {
        SceneManager.LoadScene("HubWorld");
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
