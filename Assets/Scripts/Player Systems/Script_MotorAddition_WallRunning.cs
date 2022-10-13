using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_MotorAddition_WallRunning : MonoBehaviour
{
    [SerializeField] Rigidbody RB;
    [SerializeField] float m_MinimumJumpHeight = 0.5f;
    [SerializeField] Transform Orientation;
    [SerializeField] LayerMask m_WallMask;
    [SerializeField] float m_WallDistanceCheck = 0.25f;

    [HideInInspector] public bool m_IsWallrunning = false;
    [HideInInspector] public bool m_WallLeft = false;
    [HideInInspector] public bool m_WallRight = false;
    [HideInInspector] public bool m_WallFront = false;
    [HideInInspector] public bool m_WallBack = false;

    RaycastHit m_WallLeftHit;
    RaycastHit m_WallRightHit;
    RaycastHit m_WallFrontHit;
    RaycastHit m_WallBackHit;

    Vector3 m_LastWallNormal;

    [SerializeField] private float WallRunGravity = 5;
    [SerializeField] private float WallJumpForce = 15;
    [SerializeField] private float WallStickForce = 5.0f;
    [SerializeField] private float WallJumpSideRatio = 1.5f;
    [SerializeField] private float WallJumpUpRatio = 1.0f;
    [SerializeField] private float WallJumpForwardRatio = 1.25f;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WallRun();
    }

    bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, m_MinimumJumpHeight);
    }
    void CheckWall()
    {
        m_WallLeft = Physics.Raycast(transform.position, -Orientation.right, out m_WallLeftHit, m_WallDistanceCheck, m_WallMask);
        m_WallRight = Physics.Raycast(transform.position, Orientation.right, out m_WallRightHit, m_WallDistanceCheck, m_WallMask);
        m_WallFront = Physics.Raycast(transform.position, Orientation.forward, out m_WallFrontHit, m_WallDistanceCheck, m_WallMask);
        m_WallBack = Physics.Raycast(transform.position, -Orientation.forward, out m_WallBackHit, m_WallDistanceCheck, m_WallMask);
    }

    void WallRun()
    {
        //WallRunning
        if (CanWallRun())
        {
            CheckWall();

            if (m_WallLeft || m_WallRight)
            {
                // if (m_IsWallrunning == false && RB.velocity.y < 0)
                // {
                //     RB.velocity = new Vector3(RB.velocity.x, -0, RB.velocity.z);
                // }
                StartWallRun();
                m_IsWallrunning = true;
            }
            else if (m_WallFront || m_WallBack)
            {
                StartWallRun();
                m_IsWallrunning = true;
            }
            else
            {
                RB.useGravity = true;
                m_IsWallrunning = false;
            }
        }
        else
        {
            m_IsWallrunning = false;
        }
    }

    void StartWallRun()
    {
        RB.useGravity = false;
        RB.AddForce(Vector3.down * WallRunGravity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 WallRunJumpDirection = Vector3.zero;
            if (m_WallLeft && m_LastWallNormal == m_WallLeftHit.normal)
            {
                WallRunJumpDirection = transform.up * WallJumpUpRatio * -0.5f + m_WallLeftHit.normal * WallJumpSideRatio + Orientation.forward * WallJumpForwardRatio;
                m_LastWallNormal = m_WallLeftHit.normal;
                RB.velocity = Vector3.zero;
                RB.AddForce(WallRunJumpDirection.normalized * WallJumpForce, ForceMode.VelocityChange);
                //PlayerAudio.jump();
            }
            else if (m_WallRight && m_LastWallNormal == m_WallRightHit.normal)
            {
                WallRunJumpDirection = transform.up * WallJumpUpRatio * -0.5f + m_WallRightHit.normal * WallJumpSideRatio + Orientation.forward * WallJumpForwardRatio;
                m_LastWallNormal = m_WallRightHit.normal;
                RB.velocity = Vector3.zero;
                RB.AddForce(WallRunJumpDirection.normalized * WallJumpForce, ForceMode.VelocityChange);
                //PlayerAudio.jump();
            }
            else if (m_WallFront && m_LastWallNormal == m_WallFrontHit.normal)
            {
                //WallRunJumpDirection = transform.up * WallJumpUpRatio * -0.1f + m_WallFrontHit.normal * WallJumpSideRatio + Orientation.forward * WallJumpForwardRatio;
                //m_LastWallNormal = m_WallFrontHit.normal;
                //RB.velocity = Vector3.zero;
                //RB.AddForce(WallRunJumpDirection.normalized * WallJumpForce, ForceMode.VelocityChange);
            }
            else if (m_WallBack && m_LastWallNormal == m_WallBackHit.normal)
            {
                WallRunJumpDirection = transform.up * WallJumpUpRatio * -0.5f + m_WallBackHit.normal * WallJumpSideRatio + Orientation.forward * WallJumpForwardRatio;
                m_LastWallNormal = m_WallBackHit.normal;
                RB.velocity = Vector3.zero;
                RB.AddForce(WallRunJumpDirection.normalized * WallJumpForce, ForceMode.VelocityChange);
                //PlayerAudio.jump();
            }
            else
            {
                if (RB.velocity.sqrMagnitude > 12)
                {
                    if (m_WallLeft)
                    {
                        WallRunJumpDirection = transform.up * WallJumpUpRatio + m_WallLeftHit.normal * WallJumpSideRatio + Orientation.forward * WallJumpForwardRatio;
                        m_LastWallNormal = m_WallLeftHit.normal;
                        RB.velocity = Vector3.zero;
                        RB.AddForce(WallRunJumpDirection.normalized * WallJumpForce, ForceMode.VelocityChange);
                    }
                    else if (m_WallRight)
                    {
                        WallRunJumpDirection = transform.up * WallJumpUpRatio + m_WallRightHit.normal * WallJumpSideRatio + Orientation.forward * WallJumpForwardRatio;
                        m_LastWallNormal = m_WallRightHit.normal;
                        RB.velocity = Vector3.zero;
                        RB.AddForce(WallRunJumpDirection.normalized * WallJumpForce, ForceMode.VelocityChange);
                    }
                    else if (m_WallFront)
                    {
                        WallRunJumpDirection = transform.up * WallJumpUpRatio + m_WallBackHit.normal * WallJumpSideRatio + Orientation.forward * WallJumpForwardRatio;
                        m_LastWallNormal = m_WallFrontHit.normal;
                        RB.velocity = Vector3.zero;
                        RB.AddForce(WallRunJumpDirection.normalized * WallJumpForce, ForceMode.VelocityChange);
                    }
                    else if (m_WallBack)
                    {
                        WallRunJumpDirection = transform.up * WallJumpUpRatio + m_WallBackHit.normal * WallJumpSideRatio + Orientation.forward * WallJumpForwardRatio;
                        m_LastWallNormal = m_WallBackHit.normal;
                        RB.velocity = Vector3.zero;
                        RB.AddForce(WallRunJumpDirection.normalized * WallJumpForce, ForceMode.VelocityChange);
                    }
                }
                
            }
        }
    }
}
