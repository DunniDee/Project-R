using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_AdvancedMotor : MonoBehaviour
{
private float MoveSpeed = 0;
    [SerializeField] private float WalkSpeed = 10;
    [SerializeField] private float SprintSpeed = 10;
    [SerializeField] private float CrouchSpeed = 10;
    [SerializeField] private float Acceleration = 15;
    [SerializeField] private float AirSpeedMultiplier = 0.6f;

    [SerializeField] private float GroundDrag = 3;
    [SerializeField] private float AirDrag = 1.5f;

    [SerializeField] private int MaxJumps = 2;
    int m_CurrentJumps;

    [SerializeField] private float JumpForce = 10;
    [SerializeField] private float DownForce = 7.5f;

    //[Header("Crouching")]
    [SerializeField]  private CapsuleCollider m_CapsuleCollider;
    //[Space]


    [Header("Keybinds")]
    [SerializeField] private KeyCode JumpKey = KeyCode.Space;
    [SerializeField] private KeyCode SprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode  CrouchKey = KeyCode.LeftControl;
    [Space]

    float m_HorizontalMovement;
    float m_VerticalMovement;

    [Header("Orientation")]
    [SerializeField] private Transform Orientation;
    [SerializeField] private Transform TempModel;
    [Space]

    [Header("Ground Detection")]
    [SerializeField] float m_GroundCheckSphereRadius = 0.2f;
    [SerializeField] private LayerMask GroundMask;
    [Space]

    [Header("Motor States")]
    [SerializeField]private bool m_IsGrounded;
    [SerializeField] private bool m_IsSprinting = false;
    [SerializeField] private bool m_IsCrouching = false;


    private Vector3 m_MoveDirection;
    private Vector3 m_SlopeMoveDirection;

    Rigidbody RB;
    RaycastHit m_SlopeHit;
    public Rigidbody getRigidBody()
    {
        return RB;
    }
    public bool OnSlope()
    {
        if (Physics.Raycast(Orientation.position, Vector3.down, out m_SlopeHit, 0.5f, GroundMask))
        {
            if (m_SlopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    void CheckGround()
    {
        m_IsGrounded = Physics.CheckSphere(transform.position, m_GroundCheckSphereRadius, GroundMask);

        if (OnSlope())
        {
            m_SlopeMoveDirection = Vector3.ProjectOnPlane(m_MoveDirection, m_SlopeHit.normal);
            RB.useGravity = false;
        }
        else
        {
            RB.useGravity = true;
        }
    }

    private void Start()
    {
        //initialize local variables
        RB = GetComponent<Rigidbody>();
        RB.freezeRotation = true;
    }

    private void Update()
    {
        //StatusChecks
        CheckGround();

        GetInput();
        ControlDrag();
        ControlSpeed();

        //Abilities
        Jump();
        Crouch();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void Jump()
    {
        if (Input.GetKeyDown(JumpKey))
        {
            if (m_IsGrounded)
            {
                m_CurrentJumps = MaxJumps;
            }


            if (m_CurrentJumps > 0)
            {
                RB.velocity = new Vector3(RB.velocity.x, JumpForce / 2, RB.velocity.z);
                m_CurrentJumps--;
            }
        }
    }

    void ControlDrag()
    {
        if (m_IsGrounded)
        {
            RB.drag = GroundDrag;
        }
        else
        {
            RB.drag = AirDrag;
        }
    }

    void ControlSpeed()
    {
        if (m_IsSprinting)
        {
            MoveSpeed = Mathf.Lerp(MoveSpeed, SprintSpeed, Acceleration * Time.deltaTime);
        }
        else if (m_IsCrouching)
        {
           MoveSpeed = Mathf.Lerp(MoveSpeed, CrouchSpeed, Acceleration * Time.deltaTime); 
        }
        else
        {
            MoveSpeed = Mathf.Lerp(MoveSpeed, WalkSpeed, Acceleration * Time.deltaTime);
        }
    }

    void GetInput()
    {
        m_HorizontalMovement = Input.GetAxisRaw("Horizontal");
        m_VerticalMovement = Input.GetAxisRaw("Vertical");

        m_MoveDirection = Orientation.forward * m_VerticalMovement + Orientation.right * m_HorizontalMovement;

        if (Input.GetKey(CrouchKey) && m_IsGrounded)
        {
            m_IsCrouching = true;
        }
        else
        {
            if (!Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y + m_CapsuleCollider.height,transform.position.z), 0.1f, GroundMask))
            {
                m_IsCrouching = false;   
            }
        }

        if (m_VerticalMovement > 0 && Input.GetKeyDown(SprintKey) && !m_IsCrouching || m_IsSprinting && m_VerticalMovement > 0)
        {
            m_IsSprinting = true;
        }
        else
        {
            m_IsSprinting = false;
        }
    }

    void MovePlayer()
    {
        if (m_IsGrounded && !OnSlope())
        {
            RB.AddForce(m_MoveDirection.normalized * MoveSpeed, ForceMode.Acceleration);
        }
        else if (m_IsGrounded && OnSlope())
        {
            RB.AddForce(m_SlopeMoveDirection.normalized * MoveSpeed, ForceMode.Acceleration);
        }
        else
        {
            RB.AddForce(m_MoveDirection.normalized * MoveSpeed * AirSpeedMultiplier, ForceMode.Acceleration);
        }
        
        if (RB.velocity.y < 0)
        {
            RB.AddForce(Vector3.down * DownForce, ForceMode.Acceleration);
        }
    }

    void Crouch()
    {
        if (m_IsCrouching)
        {
            m_CapsuleCollider.height = Mathf.Lerp(m_CapsuleCollider.height, 1, Time.deltaTime * 5);
            m_CapsuleCollider.center = new Vector3(0, Mathf.Lerp(m_CapsuleCollider.center.y, 0.5f, Time.deltaTime * 5),0);

            TempModel.localScale = new Vector3(1, Mathf.Lerp(  TempModel.localScale.y, 0.5f, Time.deltaTime * 5),1);
            TempModel.localPosition = new Vector3(0, Mathf.Lerp(  TempModel.localPosition.y, 0.5f, Time.deltaTime * 5),0);
        }
        else
        {
            m_CapsuleCollider.height = Mathf.Lerp(m_CapsuleCollider.height, 2, Time.deltaTime * 5);
            m_CapsuleCollider.center = new Vector3(0, Mathf.Lerp(m_CapsuleCollider.center.y, 1, Time.deltaTime * 5),0);

            TempModel.localScale = new Vector3(1, Mathf.Lerp(  TempModel.localScale.y, 1, Time.deltaTime * 5),1);
            TempModel.localPosition = new Vector3(0, Mathf.Lerp(  TempModel.localPosition.y, 1, Time.deltaTime * 5),0);
        }
    }

    //Getter functions
    public Vector3 getDirection()
    {
        return m_MoveDirection;
    }

    public Vector2 getRawDirection()
    {
        return new Vector2(m_HorizontalMovement,m_VerticalMovement);
    }

    public bool GetIsGrounded()
    {
        return m_IsGrounded;
    }

    public bool GetIsSprinting()
    {
        return m_IsSprinting;
    }

    public void SetIsSprinting(bool _input)
    {
        m_IsSprinting = _input;
    }

    public bool GetIsCrouching()
    {
        return m_IsCrouching;
    }
}
