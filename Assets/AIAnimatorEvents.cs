using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAnimatorEvents : MonoBehaviour
{
    public delegate void OnAttackDelegate(Script_BaseAI agent);
    public OnAttackDelegate OnSlashHorizontalAttack;
    public OnAttackDelegate OnSlashVerticalAttack;
    public OnAttackDelegate OnJumpAttack;
    Script_BaseAI AIagent;
    [SerializeField] AudioSource audioSource;
    [SerializeField] NavMeshAgent agent;

    [SerializeField]
    AudioClip[] FootStepClipArr;

    [SerializeField]
    AudioClip[] FireSoundClipArr;

    public void SlashHorizontal(Script_BaseAI agent)
    {
        if (OnSlashHorizontalAttack != null)
        {
            OnSlashHorizontalAttack(AIagent);
        }
    }

    public void SlashVertical(Script_BaseAI agent)
    {
        if (OnSlashVerticalAttack != null)
        {
            OnSlashVerticalAttack(AIagent);
        }
    }
    public void JumpAttack(Script_BaseAI agent)
    {
        if (OnJumpAttack != null)
        {
            OnJumpAttack(AIagent);
        }
    }
    public void PlayFootStep() { audioSource.PlayOneShot(FootStepClipArr[Random.Range(0, FootStepClipArr.Length)]); }

    public void PlayGunShot() { audioSource.PlayOneShot(FireSoundClipArr[Random.Range(0, FireSoundClipArr.Length)], 0.7f); }

    public void StopAgent() {
        agent.velocity = Vector3.zero;
        agent.isStopped = true; }
    public void StartAgent() { agent.isStopped = false; }

    // Start is called before the first frame update
    void Start()
    {
        AIagent = GetComponentInParent<Script_BaseAI>();
        agent = GetComponentInParent<NavMeshAgent>();
        audioSource = GetComponentInParent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
