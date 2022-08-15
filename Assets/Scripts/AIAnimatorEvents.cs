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

    public void SlashHorizontal()
    {
        if (OnSlashHorizontalAttack != null)
        {
            OnSlashHorizontalAttack(AIagent);
        }
    }

    public void SlashVertical()
    {
        if (OnSlashVerticalAttack != null)
        {
            OnSlashVerticalAttack(AIagent);
        }
    }
    public void JumpAttack()
    {
        if (OnJumpAttack != null)
        {
            Debug.Log("JUMPY ATTACK!");
            OnJumpAttack(AIagent);
        }
    }

   

    public void PlayFootStep() { audioSource.PlayOneShot(FootStepClipArr[Random.Range(0, FootStepClipArr.Length)]); }

    public void PlayGunShot() { audioSource.PlayOneShot(FireSoundClipArr[Random.Range(0, FireSoundClipArr.Length)], 0.7f); }

    public void StopAI()
    {
        AIagent.enabled = false;
    }
    public void StartAI()
    {
        AIagent.enabled = true;
    }
    public void StopAgent() {  agent.enabled = false; }

    public void StartAgent() { agent.enabled = true; }

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
