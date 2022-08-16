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
    [SerializeField] float m_DashRechargeTime;
    float m_DashRechargeTimer;
    float m_DashCooldownTimer;
    public int m_DashCount;
    public float m_DashMomentumTimer;

    //Motor Status Bools
    public bool m_IsGrounded;
    public bool m_IsTouchingWall;
    public bool m_WasTouchingWall;
    private bool m_WasCrouching;
    public bool m_IsCrouching;
    public bool m_IsVaulting;

    private Vector3 m_MoveDirection;
    public Vector3 m_SmoothMoveDirection;
    public Vector3 m_MomentumDirection;
    public Vector3 m_VerticalVelocity;
    float m_ForwardMovement;
    float m_SidewardMovement;

    RaycastHit LeftHit;
    RaycastHit RightHit;
    RaycastHit FrontHit;
    RaycastHit BackHit;
    RaycastHit WallHit;

    RaycastHit VaultHit;

    Vector3 WallNormal;
    [SerializeField] Transform[] WallCheckPos;

    float SlideBoostTimer;

    [SerializeField] Transform VaultCheckPos;
    Vector3 vaultPos;
    [SerializeField] float m_VaultTime;
    float m_VaultTimer;
    [SerializeField] AnimationCurve VaultCurve;

    [SerializeField] bool IsLaunched = false;
    [Header("GameFeel")]
    [SerializeField] GameObject VaultArms;
    [SerializeField] Scr_CameraEffects CamEffects;
    bool m_WasGrounded;
    float m_GroundedTimer = 0;
    float LastYVelocity;
    float m_LauncherTimer;

    [SerializeField] AudioSource StepAS;
    [SerializeField] AudioClip[] LandSounds;
    [SerializeField] AudioClip[] DashSounds;

    [SerializeField] script_WeaponSwap Weapons;


    // Update is called once per frame
    void Update()
    {
        GetMovementInput();
        CheckGround();

        WallRun();

        Jump();
        Movment.Move(m_VerticalVelocity * Time.deltaTime);

        Vault();

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

        if (m_LauncherTimer > 0)
        {
            m_LauncherTimer -= Time.deltaTime;
        }
    }

    void CheckGround()
    {
        if (Physics.CheckSphere(transform.position + (Vector3.up * 0.35f), 0.5f, GroundMask) && m_GroundedTimer < 0)
        {
           m_IsGrounded = true; 
           IsLaunched = false;
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
        if (Physics.CheckSphere(transform.position + Vector3.up,0.6f,WallRunMask | GroundMask) && !m_IsGrounded)
        {
           m_IsTouchingWall = true; 
        }
        else
        {
            m_IsTouchingWall = false; 
        }

        if (m_IsTouchingWall)
        {
            if (!m_WasTouchingWall)
            {
                IsLaunched = false;
                m_MomentumMagnuitude = Mathf.Clamp(m_MomentumMagnuitude,0, m_MomentumMax);
                m_MomentumDirection = m_MomentumDirection * m_MomentumMagnuitude;

                StepAS.PlayOneShot(LandSounds[Random.Range(0,LandSounds.Length-1)]);
            }

            m_SmoothMoveDirection = Vector3.ProjectOnPlane(m_SmoothMoveDirection, WallNormal);
            m_MomentumDirection += m_SmoothMoveDirection * Time.deltaTime;
            // m_MomentumDirection = Vector3.ProjectOnPlane(m_MomentumDirection, WallNormal);
            // Movment.Move(-WallNormal * 10 * Time.deltaTime);

            // if (Physics.Raycast(transform.position + (Vector3.up * 0.40f), Orientation.right,out RightHit, 1.5f, WallRunMask))
            // {
            //     CamEffects.RotateTo += new Vector3(0,0,45) * Time.deltaTime;
            //     WallNormal = RightHit.normal;
            //     return;
            // }

            // if (Physics.Raycast(transform.position + (Vector3.up * 0.40f), -Orientation.right,out LeftHit, 1.5f, WallRunMask))
            // {
            //     CamEffects.RotateTo += new Vector3(0,0,-45) * Time.deltaTime;
            //     WallNormal = LeftHit.normal;
            //     return;
                
            // }

            // if (Physics.Raycast(transform.position + (Vector3.up * 0.40f), Orientation.forward,out FrontHit, 1.5f, WallRunMask))
            // {
            //     CamEffects.RotateTo += new Vector3(-45,0,0) * Time.deltaTime;
            //     WallNormal = FrontHit.normal;
            //     return;
            // }

            // if (Physics.Raycast(transform.position + (Vector3.up * 0.40f), -Orientation.forward,out BackHit, 1.5f, WallRunMask))
            // {
            //     CamEffects.RotateTo += new Vector3(45,0,0) * Time.deltaTime;
            //     WallNormal = BackHit.normal;
            //     return;
            // }

            foreach (var Pos in WallCheckPos)
            {  
                RaycastHit Hit;
                if (Physics.Raycast(Pos.position, Pos.forward,out Hit, 1.5f, WallRunMask | GroundMask))
                {
                    CamEffects.RotateTo += Pos.localPosition * 100 * Time.deltaTime;
                    WallNormal = Hit.normal;
                    return;
                }
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
            if (!IsLaunched)
            {
                m_MomentumDirection = Vector3.Lerp(m_MomentumDirection, m_SmoothMoveDirection.normalized * m_MomentumMagnuitude, Time.deltaTime);
                m_MomentumDirection = Vector3.Lerp(m_MomentumDirection, Vector3.zero, Time.deltaTime * m_MomentumDecay);
            }
        }

        if (!IsLaunched)
        {
            if (m_MomentumMagnuitude > m_MomentumMax)
            {
                m_MomentumDirection = m_MomentumDirection.normalized * m_MomentumMax;
            }

            CamEffects.FovTo += m_MomentumMagnuitude * 5 * Time.deltaTime;
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
                StepAS.PlayOneShot(LandSounds[Random.Range(0,LandSounds.Length-1)]);
            }
            m_JumpCount = m_MaxJumps;
        }
        else
        {

            m_VerticalVelocity.y -= m_Gravity * Time.deltaTime;   
            m_VerticalVelocity.y = Mathf.Clamp(m_VerticalVelocity.y, -25,100000000);


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
        }

        if (Input.GetKeyDown(KeyCode.Space) && m_JumpCount > 0)
        {
            if (m_IsTouchingWall  && !m_IsGrounded)
            {
                m_MomentumDirection = Orientation.forward * m_MomentumMagnuitude * 0.5f;
                m_MomentumDirection += WallNormal * m_MomentumMagnuitude * 0.6f;
                //m_MomentumDirection += WallNormal * 10;
            }
            
            IsLaunched = false;
            m_DashMomentumTimer = 0.25f;
            m_JumpCount--;
            m_VerticalVelocity.y = Mathf.Sqrt(2 * m_JumpHeight * m_Gravity);
            CamEffects.RotateTo.x += 5;
            CamEffects.RotateTo += new Vector3(m_ForwardMovement,0,-m_SidewardMovement).normalized * m_DashMomentum;
            CamEffects.FovTo += 10;
            Movment.Move(new Vector3(0,0.15f,0));

            Weapons.JumpAnim();
        }

        float VerticalTilt = m_VerticalVelocity.y;
        VerticalTilt = Mathf.Clamp(VerticalTilt, -45,10);
        CamEffects.RotateTo.x += VerticalTilt * Time.deltaTime;
    }

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && m_DashCooldownTimer <= 0 && m_DashCount > 0 && !m_IsCrouching )
        {
            m_MomentumDirection += m_SmoothMoveDirection.normalized * m_DashMomentum ;
            CamEffects.RotateTo += new Vector3(m_ForwardMovement,0,-m_SidewardMovement).normalized * 10;
            m_DashMomentumTimer = 0.25f;
            CamEffects.FovTo += 15;
            m_DashCount--;
            m_DashCooldownTimer = m_DashCooldown;
            m_DashRechargeTimer = m_DashRechargeTime;
            IsLaunched = false;

            if (m_VerticalVelocity.y <0 )
            {
                m_VerticalVelocity.y = 0;
            }

            StepAS.PlayOneShot(DashSounds[Random.Range(0,DashSounds.Length-1)],1);
            CamEffects.ShakeTime += 0.25f;
            CamEffects.ShakeAmplitude += 0.5f;

            Weapons.DashAnim();
        }

        if (m_DashMomentumTimer > 0)
        {
            m_DashMomentumTimer-=Time.deltaTime;
        }

        if (m_DashCooldownTimer > 0)
        {
            m_DashCooldownTimer-=Time.deltaTime;
        }

        if (m_DashRechargeTimer >= 0)
        {
            m_DashRechargeTimer -= Time.deltaTime;
        }
        else
        {
            if (m_DashCount < m_MaxDashes)
            {
                m_DashRechargeTimer = m_DashRechargeTime;
                m_DashCount++;   
            }
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


            CamEffects.LerpPos = new Vector3(0,0.5f,0);
            CamEffects.RotateTo.z += 45 * Time.deltaTime;
            
            Weapons.SlideAnim(true);

            Movment.height = 1;
            Movment.center = new Vector3(0,0.5f,0);
        }
        else
        {
            CamEffects.LerpPos = new Vector3(0,1.5f,0);
            Weapons.SlideAnim(false);

            
            Movment.height = 2;
            Movment.center = new Vector3(0,1,0);
        }
    }

    void Vault()
    {
        if (Physics.Raycast(VaultCheckPos.position, VaultCheckPos.forward,out VaultHit, 1, GroundMask) && !m_IsVaulting)
        {
            m_IsVaulting = true;
            vaultPos = VaultHit.point + Vector3.up;
            m_VaultTimer = m_VaultTime;
            CamEffects.RotateTo.x -= 50;
            CamEffects.ShakeTime += 2;
            CamEffects.ShakeAmplitude += 1;
            VaultArms.SetActive(true);
            Weapons.SetActiveAnim(true);
            VaultArms.transform.rotation = Orientation.transform.rotation;
        }

        if (m_IsVaulting)
        {
            if (m_VaultTimer <= 0.5f )
            {
                m_IsVaulting = false;
                m_VerticalVelocity.y = 5;
                CamEffects.ShakeTime += 1;
                CamEffects.ShakeAmplitude += 0.25f;
                VaultArms.SetActive(false);
            }
            VaultArms.transform.position = vaultPos - Vector3.up;
            m_VaultTimer -= Time.deltaTime;
            Movment.enabled = false;
            transform.position = Vector3.Lerp( vaultPos, transform.position, VaultCurve.Evaluate(m_VaultTimer));
            Debug.Log(vaultPos);
            CamEffects.RotateTo.x += 200 * Time.deltaTime;

        }
        else
        {
            Weapons.SetActiveAnim(false);
            Movment.enabled = true;
        }
    }

    public void Launch(Vector3 _direction, float _Height)
    {
        if (m_LauncherTimer <= 0)
        {
            m_LauncherTimer = 1;
            IsLaunched = true;
            Movment.Move(new Vector3(0,0.5f,0));
            m_MomentumDirection = _direction;
            m_VerticalVelocity.y = Mathf.Sqrt(2 * _Height * m_Gravity);
            CamEffects.FovTo += 45;
            CamEffects.RotateTo += new Vector3(15,0,0);
            StepAS.PlayOneShot(DashSounds[Random.Range(0,DashSounds.Length-1)],1);
            CamEffects.ShakeTime += 1f;
            CamEffects.ShakeAmplitude += 1f;
        }
    }

    public void MovePlayer(Vector3 _Move)
    {
        Movment.Move(_Move);
    }
}
