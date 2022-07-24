using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_FootSteps : MonoBehaviour
{
    [SerializeField] Scr_CameraEffects CamEffects;
    Scr_PlayerMotor Motor;
    [SerializeField] float StepSpeed;
    [SerializeField] AudioClip[] FootSteps;
    bool IsLeft = true;
    float StepTimer;
    [SerializeField] AudioSource AS;
    float Acceleration;

    // Start is called before the first frame update
    void Start()
    {
        Motor = gameObject.GetComponent<Scr_PlayerMotor>();
        //AS = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsMoving())
        {
            Acceleration = Mathf.Lerp(Acceleration, 1, Time.deltaTime);
        }
        else
        {
           Acceleration = Mathf.Lerp(Acceleration, 0.5f, Time.deltaTime); 
        }

        if (Motor.m_IsGrounded && !Motor.m_IsCrouching && IsMoving() || Motor.m_IsTouchingWall && !Motor.m_IsGrounded)
        {
            if (StepTimer > 0)
            {
                StepTimer -= Time.deltaTime * Acceleration + (Motor.m_MomentumMagnuitude * 0.00025f);
            }
            else
            {
                StepTimer = StepSpeed;
                Step();
            }
        }
        else
        {
            StepTimer = StepSpeed * 1.5f;
        }
    }

    void Step()
    {
        IsLeft = !IsLeft;
        if (IsLeft)
        {
           CamEffects.RotateTo.y += 1.5f;
        }
        else
        {
            CamEffects.RotateTo.y -= 1.5f; 
        }
        CamEffects.RotateTo.x += 1.5f;
        //CamEffects.LerpTo.y -= 0.075f;
        AS.PlayOneShot(FootSteps[Random.Range(0,FootSteps.Length-1)]);
    }

    bool IsMoving()
    {
        if (Motor.m_SmoothMoveDirection.sqrMagnitude < 0.5f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
