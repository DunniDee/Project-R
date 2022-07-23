using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlayerMotor : MonoBehaviour
{
    [Header("Core Components")]
    [SerializeField] CharacterController Movment;
    [SerializeField] Transform Orientation;
    [SerializeField] LayerMask GroundMask;
    [SerializeField] LayerMask WallRunMask;
    [Space]

    [Header("Movement Stats")]
    [SerializeField] float MoveSpeed;
    [SerializeField] float AirSpeed;
    [SerializeField] float Acceleration;
    [SerializeField] float m_MomentumMax;
    [SerializeField] float m_MomentumDecay;
    public float m_MomentumMagnuitude;
    public Vector3 LastPos;
    float m_MovementSpeed;
    [Space]

    [Header("Jump Stats")]
    [SerializeField] int m_MaxJumps;
     int m_JumpCount;
    [SerializeField] float m_Gravity;
    [SerializeField] float m_JumpMomentum;
    [SerializeField] float m_JumpHeight;
    [Space]

    [Header("Dash Stats")]
    [SerializeField] float m_DashMomentum;
    [SerializeField] int m_MaxDashes;
    [SerializeField] float m_DashCooldown;
    float m_DashCooldownTimer;
    int m_DashCount;
    public float m_DashMomentumTimer;

    //Motor Status Bools
    public bool m_IsGrounded;
    public bool m_IsTouchingWall;
    public bool m_WasTouchingWall;
    private bool m_WasCrouching;
    public bool m_IsCrouching;

    private Vector3 m_MoveDirection;
    public Vector3 m_SmoothMoveDirection;
    public Vector3 m_MomentumDirection;
    public Vector3 m_VerticalVelocity;
    float m_ForwardMovement;
    float m_SidewardMovement;

    [Header("Wall Running")]
    [SerializeField] Transform FrontPos;

    RaycastHit LeftHit;
    RaycastHit RightHit;
    RaycastHit FrontHit;
    RaycastHit BackHit;
    RaycastHit WallHit;

    Vector3 WallNormal;

    float SlideBoostTimer;
    


    [Header("GameFeel")]
    [SerializeField] Scr_CameraEffects CamEffects;
    bool m_WasGrounded;
    float m_GroundedTimer = 0;
    float LastYVelocity;

    // Update is called once per frame
    void Update()
    {
        GetMovementInput();
        CheckGround();

        WallRun();

        Jump();
        Movment.Move(m_VerticalVelocity * Time.deltaTime);

        Dash();
        Crouch();

        SmoothMomentum();
        SmoothMovment();

        MoveMotor();

        m_WasGrounded = m_IsGrounded;
        m_WasTouchingWall = m_IsTouchingWall;
        LastYVelocity = m_VerticalVelocity.y;
        m_WasCrouching = m_IsCrouching;
        LastPos = transform.position;
    }

    void CheckGround()
    {
        if (Physics.CheckSphere(transform.position + (Vector3.up * 0.35f), 0.5f, GroundMask) && m_GroundedTimer < 0)
        {
           m_IsGrounded = true; 
        }
        else
        {
            m_IsGrounded = false; 
        }


        if (m_GroundedTimer >= 0)
        {
            m_GroundedTimer -= Time.deltaTime;
        }
    }

    void WallRun()
    {
        if (Physics.CheckSphere(transform.position + Vector3.up,0.6f,WallRunMask))
        {
           m_IsTouchingWall = true; 
        }
        else
        {
            m_IsTouchingWall = false; 
        }

        if (m_IsTouchingWall)
        {
            if (Physics.Raycast(transform.position + (Vector3.up * 0.40f), Orientation.right,out RightHit, 1.5f, WallRunMask))
            {
                CamEffects.RotateTo += new Vector3(0,0,45) * Time.deltaTime;
                WallNormal = RightHit.normal;
            }

            if (Physics.Raycast(transform.position + (Vector3.up * 0.40f), -Orientation.right,out LeftHit, 1.5f, WallRunMask))
            {
                CamEffects.RotateTo += new Vector3(0,0,-45) * Time.deltaTime;
                WallNormal = LeftHit.normal;
            }

            if (Physics.Raycast(transform.position + (Vector3.up * 0.40f), Orientation.forward,out FrontHit, 1.5f, WallRunMask))
            {
                CamEffects.RotateTo += new Vector3(-45,0,0) * Time.deltaTime;
                WallNormal = FrontHit.normal;
            }

            if (Physics.Raycast(transform.position + (Vector3.up * 0.40f), -Orientation.forward,out BackHit, 1.5f, WallRunMask))
            {
                CamEffects.RotateTo += new Vector3(45,0,0) * Time.deltaTime;
                WallNormal = BackHit.normal;
            }
        }


    }

    void SmoothMovment()
    {
        if (!m_IsGrounded || m_IsCrouching)
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
        m_MomentumMagnuitude = m_MomentumDirection.magnitude;

        if (m_IsGrounded && !m_IsCrouching && m_DashMomentumTimer <= 0)
        {
            m_MomentumDirection = Vector3.Lerp(m_MomentumDirection,Vector3.zero, Time.deltaTime * 5);
        }
        else
        {
            m_MomentumDirection = Vector3.Lerp(m_MomentumDirection, m_SmoothMoveDirection.normalized * m_MomentumMagnuitude, Time.deltaTime * 1);
            m_MomentumDirection = Vector3.Lerp(m_MomentumDirection, Vector3.zero, Time.deltaTime * m_MomentumDecay);
            
        }

        if (m_MomentumMagnuitude > m_MomentumMax)
        {
            m_MomentumDirection = m_MomentumDirection.normalized * m_MomentumMax;
        }

        CamEffects.FovTo += m_MomentumMagnuitude * 5 * Time.deltaTime;
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
            m_JumpCount = m_MaxJumps;
        }
        else
        {
            m_VerticalVelocity.y -= m_Gravity * Time.deltaTime;

            if (m_IsTouchingWall)
            {
                m_VerticalVelocity.y = Mathf.Clamp(m_VerticalVelocity.y, -1, 1000000);
            }

            if (m_WasGrounded && !m_IsCrouching )
            {
                m_MomentumDirection += m_SmoothMoveDirection * m_MovementSpeed;
            }
        }

        if (m_IsTouchingWall)
        {
            if (!m_WasTouchingWall)
            {
                CamEffects.RotateTo.x += 15;
            }
            m_JumpCount = m_MaxJumps;
            m_DashCount = m_MaxDashes;
        }

        if (Input.GetKeyDown(KeyCode.Space) && m_JumpCount > 0)
        {
            if (m_IsTouchingWall  && !m_IsGrounded)
            {
                m_MomentumDirection += m_SmoothMoveDirection.normalized * m_DashMomentum * 0.25f;
                m_MomentumDirection += WallNormal * 10;
            }

            m_DashMomentumTimer = 0.25f;
            m_JumpCount--;
            m_VerticalVelocity.y = Mathf.Sqrt(2 * m_JumpHeight * m_Gravity);
            CamEffects.RotateTo.x += 5;
            CamEffects.RotateTo += new Vector3(m_ForwardMovement,0,-m_SidewardMovement).normalized * m_DashMomentum;
            CamEffects.FovTo += 10;
        }

        float VerticalTilt = m_VerticalVelocity.y;
        VerticalTilt = Mathf.Clamp(VerticalTilt, -45,10);
        CamEffects.RotateTo.x += VerticalTilt * Time.deltaTime;
    }

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && m_DashCooldownTimer <= 0 && m_DashCount > 0 && !m_IsCrouching)
        {
            //Movment.Move(m_SmoothMoveDirection.normalized * 1);
            m_MomentumDirection += m_SmoothMoveDirection.normalized * m_DashMomentum ;
            CamEffects.RotateTo += new Vector3(m_ForwardMovement,0,-m_SidewardMovement).normalized * 10;
            m_DashMomentumTimer = 0.25f;
            CamEffects.FovTo += 10;
            m_DashCount--;
            m_DashCooldownTimer = m_DashCooldown;

            if (m_VerticalVelocity.y <0 )
            {
                m_VerticalVelocity.y = 0;
            }
        }

        if (m_DashMomentumTimer > 0)
        {
            m_DashMomentumTimer-=Time.deltaTime;
        }

        if (m_DashCooldownTimer > 0)
        {
            m_DashCooldownTimer-=Time.deltaTime;
        }

        if (m_IsGrounded)
        {
            m_DashCount = m_MaxDashes;
        }
    }

    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            m_IsCrouching = true;
        }

        if (!Input.GetKey(KeyCode.LeftControl) && !Physics.CheckSphere(transform.position + (Vector3.up * 1.5f),0.45f, GroundMask))
        {
            m_IsCrouching = false;
        }

        if (SlideBoostTimer >= 0)
        {
            SlideBoostTimer-= Time.deltaTime;
        }

        if (m_IsCrouching && m_IsGrounded)
        {
            if (m_IsGrounded && !m_WasCrouching && SlideBoostTimer <0)
            {
                m_MomentumDirection += m_SmoothMoveDirection * m_MovementSpeed;
                SlideBoostTimer = 1;
            }


            CamEffects.LerpPos = new Vector3(0,-1,0);
            CamEffects.RotateTo.z += 25 * Time.deltaTime;

            Movment.height = 1;
            Movment.center = new Vector3(0,0.5f,0);
        }
        else
        {
            CamEffects.LerpPos = new Vector3(0,0,0);
            
            Movment.height = 2;
            Movment.center = new Vector3(0,1,0);
        }
    }
}
