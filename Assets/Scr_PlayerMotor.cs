using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlayerMotor : MonoBehaviour
{
    [Header("Core Components")]
    [SerializeField] CharacterController Movment;
    [SerializeField] Transform Orientation;
    [SerializeField] LayerMask GroundMask;
    [Space]

    [Header("Movement Stats")]
    [SerializeField] float MoveSpeed;
    [SerializeField] float AirSpeed;
    [SerializeField] float Acceleration;
    [SerializeField] float m_MomentumAcceleration;
    [SerializeField] float m_MomentumDecay;
    float m_MovementSpeed;
    [Space]

    [Header("Jump Stats")]
    [SerializeField] float m_Gravity;
    [SerializeField] float m_JumpMomentum;
    [SerializeField] float m_JumpHeight;

    //Motor Status Bools
    public bool m_IsGrounded;

    private Vector3 m_MoveDirection;
    private Vector3 m_SmoothMoveDirection;
    public Vector3 m_MomentumDirection;
    private Vector3 m_VerticalVelocity;
    float m_ForwardMovement;
    float m_SidewardMovement;

    [Header("GameFeel")]
    [SerializeField] Scr_CameraEffects CamEffects;
    bool m_WasGrounded;
    float LastYVelocity;

    // Update is called once per frame
    void Update()
    {
        GetMovementInput();
        CheckGround();

        Jump();
        Movment.Move(m_VerticalVelocity * Time.deltaTime);

        SmoothMomentum();
        SmoothMovment();

        MoveMotor();

        m_WasGrounded = m_IsGrounded;
        LastYVelocity = m_VerticalVelocity.y;
    }

    void CheckGround()
    {
        if (Physics.CheckSphere(transform.position + (Vector3.up * 0.2f),0.3f,GroundMask))
        {
           m_IsGrounded = true; 
        }
        else
        {
            m_IsGrounded = false; 
        }
    }

    void SmoothMovment()
    {
        if (!m_IsGrounded)
        {
            m_MovementSpeed = Mathf.Lerp(m_MovementSpeed, AirSpeed, Time.deltaTime * Acceleration);
        }
        else if (m_MoveDirection == Vector3.zero)
        {
            m_MovementSpeed = Mathf.Lerp(m_MovementSpeed, 0, Time.deltaTime * Acceleration);
        }
        else
        {
            m_MovementSpeed = Mathf.Lerp(m_MovementSpeed, MoveSpeed, Time.deltaTime * Acceleration);
        }

        m_SmoothMoveDirection = Vector3.Lerp(m_SmoothMoveDirection, new Vector3(m_MoveDirection.x,0,m_MoveDirection.z).normalized, Time.deltaTime * Acceleration);

        CamEffects.RotateTo += new Vector3(m_ForwardMovement,0,-m_SidewardMovement) * 10 * Time.deltaTime;
    }

    void SmoothMomentum()
    {
        if (m_IsGrounded)
        {
            m_MomentumDirection = Vector3.Lerp(m_MomentumDirection,Vector3.zero, Time.deltaTime * 10);
        }
        else
        {
            m_MomentumDirection = Vector3.Lerp(m_MomentumDirection, m_SmoothMoveDirection.normalized * m_MomentumDirection.magnitude, Time.deltaTime * 1);
            m_MomentumDirection = Vector3.Lerp(m_MomentumDirection, Vector3.zero, Time.deltaTime * m_MomentumDecay);
        }
    }


    void GetMovementInput()
    {
        m_SidewardMovement = Input.GetAxisRaw("Horizontal");
        m_ForwardMovement = Input.GetAxisRaw("Vertical");

        m_MoveDirection = Orientation.forward * m_ForwardMovement + Orientation.right * m_SidewardMovement;
    }

    void MoveMotor()
    {
        Movment.Move(m_SmoothMoveDirection * m_MovementSpeed * Time.deltaTime);
        Movment.Move(m_MomentumDirection * Time.deltaTime);
    }

    void Jump()
    {
        if (m_IsGrounded)
        {
            m_VerticalVelocity.y = 0;

            if (!m_WasGrounded)
            {
                CamEffects.RotateTo.x += Mathf.Abs(LastYVelocity);
            }
        }
        else
        {
            m_VerticalVelocity.y -= m_Gravity * Time.deltaTime;

            if (m_WasGrounded)
            {
                m_MomentumDirection = m_SmoothMoveDirection * m_MovementSpeed;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_VerticalVelocity.y = Mathf.Sqrt(2 * m_JumpHeight * m_Gravity);
            CamEffects.RotateTo.x += 5;
            m_MomentumDirection += m_SmoothMoveDirection * m_JumpMomentum;
        }

        float VerticalTilt = m_VerticalVelocity.y;
        VerticalTilt = Mathf.Clamp(VerticalTilt, -45,10);
        CamEffects.RotateTo.x += VerticalTilt * Time.deltaTime;
    }

    
}
