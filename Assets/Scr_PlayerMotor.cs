using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PlayerMotor : MonoBehaviour
{
    [SerializeField] CharacterController Movment;
    [SerializeField] Transform Orientation;


    [SerializeField] float WalkSpeed;
    [SerializeField] float SprintSpeed;
    [SerializeField] float Acceleration;
    float m_MovementSpeed;
    bool m_IsSprinting;

    private Vector3 m_MoveDirection;
    private Vector3 m_SmoothMoveDirection;
    float m_ForwardMovement;
    float m_SidewardMovement;

    // Update is called once per frame
    void Update()
    {
        GetInput();
        SmoothMovment();

        Movment.Move(m_SmoothMoveDirection * m_MovementSpeed * Time.deltaTime);

    }

    void SmoothMovment()
    {
        if (m_IsSprinting)
        {
            m_MovementSpeed = Mathf.Lerp(m_MovementSpeed,SprintSpeed, Time.deltaTime * Acceleration);
        }
        else
        {
            m_MovementSpeed = Mathf.Lerp(m_MovementSpeed,WalkSpeed, Time.deltaTime * Acceleration);
        }

        m_SmoothMoveDirection = Vector3.Lerp(m_SmoothMoveDirection, new Vector3(m_MoveDirection.x,0,m_MoveDirection.z).normalized, Time.deltaTime * Acceleration);
    }


    void GetInput()
    {
        m_SidewardMovement = Input.GetAxisRaw("Horizontal");
        m_ForwardMovement = Input.GetAxisRaw("Vertical");

        m_MoveDirection = Orientation.forward * m_ForwardMovement + Orientation.right * m_SidewardMovement;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            m_IsSprinting = true;
        }

        if (m_SmoothMoveDirection == Vector3.zero)
        {
            m_IsSprinting = false;
        }
    }
}
