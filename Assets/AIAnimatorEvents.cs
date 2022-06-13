using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimatorEvents : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    [SerializeField]
    AudioClip[] FootStepClipArr;

    [SerializeField]
    AudioClip[] FireSoundClipArr;
    public void PlayFootStep() { audioSource.PlayOneShot(FootStepClipArr[Random.Range(0, FootStepClipArr.Length)]); }

    public void PlayGunShot() { audioSource.PlayOneShot(FireSoundClipArr[Random.Range(0, FireSoundClipArr.Length)]); }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponentInParent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
