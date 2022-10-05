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
    public float MoveSpeed;
    public float AirSpeed;
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
    public Vector3 m_TertiaryVelocity;

    float m_ForwardMovement;
    float m_SidewardMovement;

    RaycastHit m_VaultHit;

    Vector3 m_WallNormal;
    Vector3 m_LastWallNormal;
    [SerializeField] Transform[] WallCheckPos;

    float m_SlideBoostTimer;

    [SerializeField] Transform VaultCheckPos;
    Vector3 m_VaultPos;
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

    Vector3 m_GroundNormal;

    [SerializeField] AudioSource StepAS;
    [SerializeField] AudioClip[] LandSounds;
    [SerializeField] AudioClip[] DashSounds;

    [SerializeField] script_WeaponSwap Weapons;
    [SerializeField] Transform CamTransform;

    // Update is called once per frame
    void Update()
    {
        GetMovementInput();
        CheckGround();

        WallRun();

        Jump();

        Vault();

        Dash();
        Crouch();

        SmoothMomentum();
        SmoothMovment();

        MoveMotor();
        
        //settiing the post frame stats
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

    /// <summary>
    /// Checks for the grounded tag below the player and sets m_IsGrounded accordingly
    /// </summary>
    void CheckGround() 
    {
        // Checks for ground
        if (Physics.CheckSphere(transform.position + (Vector3.up * 0.3f), 0.5f, GroundMask) && m_GroundedTimer < 0)
        {
           m_IsGrounded = true; 
           IsLaunched = false;
        }
        else
        {
            m_IsGrounded = false; 
        }

        if (Physics.CheckSphere(CamTransform.position + (Vector3.up * 0.3f), 0.35f, GroundMask))
        {
           if (m_VerticalVelocity.y > 0)
           {
                m_VerticalVelocity.y = 0;
           }
           Debug.Log("Hit Head");
        }

        if (m_GroundedTimer >= 0)
        {
            m_GroundedTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Wall running mechanic, checks if wallrunning is possible then applies neccsisary aterations when possible
    /// </summary>
    void WallRun()
    { 
        //initial check for contact with wall
        if (Physics.CheckSphere(transform.position + Vector3.up,0.6f,GroundMask) && !m_IsGrounded)
        {
           m_IsTouchingWall = true; 
        }
        else
        {
            m_IsTouchingWall = false; 
        }

        if (m_IsTouchingWall) // when wall running
        {
            if (!m_WasTouchingWall) // one off  for audio and 
            {
                IsLaunched = false;
                m_MomentumMagnuitude = Mathf.Clamp(m_MomentumMagnuitude,0, m_MomentumMax);
                m_MomentumDirection = m_MomentumDirection * m_MomentumMagnuitude;

                StepAS.PlayOneShot(LandSounds[Random.Range(0,LandSounds.Length-1)]);
            }
            
            // project movement onto the wall norma;
            m_SmoothMoveDirection = Vector3.ProjectOnPlane(m_SmoothMoveDirection, m_WallNormal);
            m_MomentumDirection += m_SmoothMoveDirection * Time.deltaTime; // slight boost in momentum

            foreach (var Pos in WallCheckPos) // checking all the wall positiopns
            {  
                RaycastHit Hit;
                if (Physics.Raycast(Pos.position, Pos.forward,out Hit, 1.5f, WallRunMask | GroundMask)) // setting the current wall normal and the rotation based off of the normal
                {
                    CamEffects.RotateTo += Pos.localPosition * 100 * Time.deltaTime;
                    m_WallNormal = Hit.normal;
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Smoothly interpolates between all the different speeds based on status indicators e.g m_IsGrounded, m_IsCrouching
    /// </summary>
    void SmoothMovment()
    {
        if (!m_IsGrounded || m_IsCrouching) // if in air
        {
            m_MovementSpeed = Mathf.Lerp(m_MovementSpeed, AirSpeed, Time.deltaTime * Acceleration);
        }
        else if (m_MoveDirection == Vector3.zero) // if not putting any input
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

    /// <summary>
    /// Smoothly adds and decays the momentum based on weather the player us airborne 
    /// </summary>
    void SmoothMomentum() // smoothing for the motor
    {
        m_MomentumMagnuitude = m_MomentumDirection.magnitude; // getting the magnitude once since its quite a costly funcxtion

        if (m_IsGrounded && !m_IsCrouching && m_DashMomentumTimer <= 0) // if grounded
        {
            // slow the momentum down
            m_MomentumDirection = Vector3.Lerp(m_MomentumDirection,Vector3.zero, Time.deltaTime * 5);
        }
        else
        {
            if (!IsLaunched) // if is launched  then ignore decay
            {
                m_MomentumDirection = Vector3.Lerp(m_MomentumDirection, m_SmoothMoveDirection.normalized * m_MomentumMagnuitude, Time.deltaTime); // slowly rotate the momentum to the input
                m_MomentumDirection = Vector3.Lerp(m_MomentumDirection, Vector3.zero, Time.deltaTime * m_MomentumDecay);// slowly decay the momentum
            }
        }

        if (!IsLaunched)
        {
            if (m_MomentumMagnuitude > m_MomentumMax) // capping the momentum to the maximum
            {
                m_MomentumDirection = m_MomentumDirection.normalized * m_MomentumMax;
            }

            CamEffects.FovTo += m_MomentumMagnuitude * 5 * Time.deltaTime; // fov based off of the momentum
        }
        else
        {
             CamEffects.FovTo += m_MomentumMagnuitude * 5 * Time.deltaTime; 
        }

    }

    /// <summary>
    /// sets the movment vector
    /// </summary>
    void GetMovementInput() // get inputs
    {
        m_SidewardMovement = Input.GetAxisRaw("Horizontal");
        m_ForwardMovement = Input.GetAxisRaw("Vertical");

        m_MoveDirection = Orientation.forward * m_ForwardMovement + Orientation.right * m_SidewardMovement;
    }

    /// <summary>
    /// Moves the motor every frame
    /// </summary>
    void MoveMotor() // movement
    {   
        // checks to see if on slope
        RaycastHit GroundHit;
        if (Physics.Raycast(transform.position, Vector3.down,out GroundHit,2,GroundMask))
        {
            m_GroundNormal = GroundHit.normal;
            m_SmoothMoveDirection = Vector3.ProjectOnPlane(m_SmoothMoveDirection,m_GroundNormal);
            m_MomentumDirection = Vector3.ProjectOnPlane(m_MomentumDirection,m_GroundNormal);
        }

        // horizontal movement
        Movment.Move(m_SmoothMoveDirection * m_MovementSpeed * Time.deltaTime);
        Movment.Move(m_MomentumDirection * Time.deltaTime);
        Movment.Move(m_TertiaryVelocity * Time.deltaTime);

    }

    /// <summary>
    /// Jump and gravity functionality
    /// </summary>
    void Jump()
    {
        Movment.Move(m_VerticalVelocity * Time.deltaTime); // apply gravity

        if (m_IsGrounded) // is toughing ground
        {
            m_VerticalVelocity.y = 0; // set velocity to 0

            if (!m_WasGrounded) // on land play cam animation 
            {
                CamEffects.RotateTo.x += Mathf.Abs(LastYVelocity);
                StepAS.PlayOneShot(LandSounds[Random.Range(0,LandSounds.Length-1)]);
            }

            m_JumpCount = m_MaxJumps; // reset jumps
        }
        else
        {
            if (m_IsTouchingWall) // if wall running then set gravity low
            {
                m_VerticalVelocity.y = Mathf.Clamp(m_VerticalVelocity.y, -15, 5);
                m_VerticalVelocity.y -= m_Gravity * 0.25f * Time.deltaTime;   
            }
            else
            { // clamp velocity when falling teminal velocity
                m_VerticalVelocity.y -= m_Gravity * Time.deltaTime;   
                m_VerticalVelocity.y = Mathf.Clamp(m_VerticalVelocity.y, -50,100000000);
            }

            if (m_WasGrounded && !m_IsCrouching ) // shift momentum 
            {
                m_MomentumDirection += m_SmoothMoveDirection * m_MovementSpeed;
            }
        }

        if (m_IsTouchingWall) // when wall running
        {
            if (!m_WasTouchingWall) // one off wall running effect
            {
                CamEffects.RotateTo.x += 15;
                if (m_VerticalVelocity.y < 1)
                {
                    m_VerticalVelocity.y = 1;
                }
            }

            // reset jump count
            m_JumpCount = m_MaxJumps;
        }

        if (Input.GetKeyDown(KeyCode.Space) && m_JumpCount > 0)
        {
            IsLaunched = false;
            m_DashMomentumTimer = 0.25f;
            m_JumpCount--;
            m_VerticalVelocity.y = Mathf.Sqrt(2 * m_JumpHeight * m_Gravity);
            CamEffects.RotateTo.x += 5;
            CamEffects.RotateTo += new Vector3(m_ForwardMovement,0,-m_SidewardMovement).normalized * m_DashMomentum;
            CamEffects.FovTo += 10;
            Movment.Move(new Vector3(0,0.25f,0));
            Weapons.JumpAnim();

            //is touching wall
            if (m_IsTouchingWall  && !m_IsGrounded)
            {
                m_MomentumDirection = Orientation.forward * m_MomentumMagnuitude * 0.5f;
                m_MomentumDirection += m_WallNormal * m_MomentumMagnuitude * 0.6f;
                m_MomentumDirection += m_WallNormal * 5;
                m_LastWallNormal = m_WallNormal;

                if (m_WallNormal != m_LastWallNormal) // to prevent the ability to keep junmping up on the ame wall
                {
                    m_VerticalVelocity.y = Mathf.Sqrt(2 * m_JumpHeight * m_Gravity);
                }
            }
            else // Add jump force
            {
                m_VerticalVelocity.y = Mathf.Sqrt(2 * m_JumpHeight * m_Gravity);
            }
            
        }


        // cam effects
        float VerticalTilt = m_VerticalVelocity.y;
        VerticalTilt = Mathf.Clamp(VerticalTilt, -45,10);
        CamEffects.RotateTo.x += VerticalTilt * Time.deltaTime;
    }

    /// <summary>
    /// Dash functionality ads a small impulse, redirects mometum and resets vertical velocity
    /// </summary>
    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && m_DashCooldownTimer <= 0 && m_DashCount > 0 && !m_IsCrouching ) // dash if key press, has cooldown, has enough dashes and not dashing
        {
            if (m_VerticalVelocity.y <0 ) // if vertical velocity is negative reset to 0
            {
                m_VerticalVelocity.y = -1;
            }

            // adds momentum based on movement direction
            m_MomentumDirection += m_SmoothMoveDirection.normalized * m_DashMomentum;
            m_DashMomentumTimer = 0.25f;

            
            m_DashCount--;
            m_DashCooldownTimer = m_DashCooldown;
            m_DashRechargeTimer = m_DashRechargeTime;
            IsLaunched = false;

            StepAS.PlayOneShot(DashSounds[Random.Range(0,DashSounds.Length-1)],1);

            CamEffects.RotateTo += new Vector3(m_ForwardMovement,0,-m_SidewardMovement).normalized * 10;
            CamEffects.FovTo += 15;
            CamEffects.ShakeTime += 0.25f;
            CamEffects.ShakeAmplitude += 0.5f;

            Weapons.DashAnim();
        }

        // counters for the cooldowns
        if (m_DashMomentumTimer > 0)
        {
            m_DashMomentumTimer-=Time.deltaTime;
        }

        if (m_DashCooldownTimer > 0)
        {
            m_DashCooldownTimer-=Time.deltaTime;
        }


        // cooldown for the dash cooldown
        // coolsdown if charges are missing
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

    /// <summary>
    /// Crouching and sliding functionality
    /// </summary>
    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl)) // attempting to crouch
        {
            m_IsCrouching = true;
        }

        if (!Input.GetKey(KeyCode.LeftControl) && !Physics.CheckSphere(transform.position + (Vector3.up * 1.5f),0.45f, GroundMask)) // uncrouches when non grounded  or key up of crouch key
        {
            m_IsCrouching = false;
        }

        if (m_SlideBoostTimer >= 0)
        {
            m_SlideBoostTimer-= Time.deltaTime;
        }

        if (m_IsCrouching && m_IsGrounded) // if is grounded then crouch
        {
            if (m_IsGrounded && !m_WasCrouching && m_SlideBoostTimer <0) // one off boost to speed
            {
                m_MomentumDirection += m_SmoothMoveDirection * m_MovementSpeed;
                m_SlideBoostTimer = 1; // cooldown for the speed boost
            }

            Vector3 SlopeDir = m_GroundNormal + Vector3.down;
            m_MomentumDirection += SlopeDir * 50 * Time.deltaTime;
            

            CamEffects.LerpPos = new Vector3(0,0.5f,0);
            CamEffects.RotateTo.z += 45 * Time.deltaTime;
            
            Weapons.SlideAnim(true);

            Movment.height = 1;
            Movment.center = new Vector3(0,0.5f,0);
        }
        else // unsliding
        {
            CamEffects.LerpPos = new Vector3(0,1.5f,0);
            Weapons.SlideAnim(false);

            
            Movment.height = 2;
            Movment.center = new Vector3(0,1,0);
        }
    }

    /// <summary>
    /// Vaulting functionality, lerps position between current position and avaliable lerp positon 
    /// </summary>
    void Vault()
    {
        if (Physics.Raycast(VaultCheckPos.position, VaultCheckPos.forward,out m_VaultHit, 1, GroundMask) && !m_IsVaulting) //checks to see if vault is possible
        {
            m_IsVaulting = true;
            m_VaultPos = m_VaultHit.point + Vector3.up; // setting the lerp to position to lerp to
            m_VaultTimer = m_VaultTime; 
            CamEffects.RotateTo.x -= 50;
            CamEffects.ShakeTime += 2;
            CamEffects.ShakeAmplitude += 1;
            VaultArms.SetActive(true);
            Weapons.SetActiveAnim(true);
            VaultArms.transform.rotation = Orientation.transform.rotation;
        }

        if (m_IsVaulting) // when vaulting - lerping between current position to the set lerp positi0on
        {
            if (m_VaultTimer <= 0.5f )
            {
                m_IsVaulting = false;
                m_VerticalVelocity.y = 5;
                CamEffects.ShakeTime += 1;
                CamEffects.ShakeAmplitude += 0.25f;
                VaultArms.SetActive(false);
            }
            VaultArms.transform.position = m_VaultPos - Vector3.up;
            m_VaultTimer -= Time.deltaTime;
            Movment.enabled = false;
            transform.position = Vector3.Lerp( m_VaultPos, transform.position, VaultCurve.Evaluate(m_VaultTimer));
            Debug.Log(m_VaultPos);
            CamEffects.RotateTo.x += 200 * Time.deltaTime;

        }
        else // renable motor and weapons
        {
            Weapons.SetActiveAnim(false);
            Movment.enabled = true;
        }
    }

    /// <summary>
    /// launches the player and turns off any momentum decay 
    /// </summary>
    public void Launch(Vector3 _direction, float _Height)
    {
        if (m_LauncherTimer <= 0)
        {
            m_LauncherTimer = 1;
            IsLaunched = true;
            Movment.Move(new Vector3(0,0.5f,0));
            m_MomentumDirection = _direction;
            m_SmoothMoveDirection = Vector3.zero;
            m_VerticalVelocity.y = Mathf.Sqrt(2 * _Height * m_Gravity);
            CamEffects.FovTo += 45;
            CamEffects.RotateTo += new Vector3(15,0,0);
            StepAS.PlayOneShot(DashSounds[Random.Range(0,DashSounds.Length-1)],1);
            CamEffects.ShakeTime += 1f;
            CamEffects.ShakeAmplitude += 1f;
        }
    }

    public void ExplosionForce(Vector3 _direction)
    {
        // if (m_LauncherTimer <= 0)
        // {
            //m_LauncherTimer = 1;
            //IsLaunched = true;
            Movment.Move(new Vector3(0,0.5f,0));
            m_MomentumDirection += _direction;
            //m_SmoothMoveDirection = Vector3.zero;
            m_VerticalVelocity.y += _direction.y;
            CamEffects.FovTo += 45;
            CamEffects.RotateTo += new Vector3(45,0,0);
            StepAS.PlayOneShot(DashSounds[Random.Range(0,DashSounds.Length-1)],1);
            CamEffects.ShakeTime += 1f;
            CamEffects.ShakeAmplitude += 1f;
        // }
    }

    /// <summary>
    /// helper function 
    /// </summary>
    public void MovePlayer(Vector3 _Move)
    {
        Movment.Move(_Move);
    }
}
