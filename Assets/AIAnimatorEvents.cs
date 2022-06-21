using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAnimatorEvents : MonoBehaviour
{
    public delegate void OnAttackDelegate(Script_BaseAI agent);
    public OnAttackDelegate OnAttackEvent;

    [SerializeField] AudioSource audioSource;
    [SerializeField] NavMeshAgent agent;

    [SerializeField]
    AudioClip[] FootStepClipArr;

    [SerializeField]
    AudioClip[] FireSoundClipArr;

    public void Attack(Script_BaseAI agent)
    {
        if (OnAttackEvent != null)
        {
            OnAttackEvent(GetComponentInParent<Script_BaseAI>());
        }
    }
    public void PlayFootStep() { audioSource.PlayOneShot(FootStepClipArr[Random.Range(0, FootStepClipArr.Length)]); }

    public void PlayGunShot() { audioSource.PlayOneShot(FireSoundClipArr[Random.Range(0, FireSoundClipArr.Length)]); }

    public void StopAgent() {
        agent.velocity = Vector3.zero;
        agent.isStopped = true; }
    public void StartAgent() { agent.isStopped = false; }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        audioSource = GetComponentInParent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
