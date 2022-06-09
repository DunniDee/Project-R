using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_ShipController : MonoBehaviour
{
    [Header("Internal Components")]
    public Rigidbody rigidBody;

    [Header("Movement Properties")]
    private float m_maxSprintSpeed = 10f;
    private float m_maxMoveSpeed = 6f;

    private float m_breakSpeed = 3.5f;
    private float m_accelerationSpeed = 2.5f;
    public float curMoveSpeed = 0f;

    public bool isAccelerating = false;
    public bool isBreaking = false;

    private bool isInputEnabled = true;

    public Vector3 input_LookDir;
    [Header("Rotation Properties")]
    public float rotationSpeed = 10f;

    [Header("Input Properties")]
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode BrakeKey = KeyCode.Space;
    public KeyCode forwardKey = KeyCode.W;
    public KeyCode backwardKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;

    void ProcessInput()
    {
        input_LookDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (Input.GetKey(sprintKey) && !isBreaking)
        {
            isAccelerating = true;
        }
        else if(Input.GetKeyUp(sprintKey))
        {
            isAccelerating = false;
        }

        if(Input.GetKey(BrakeKey))
        {
            isBreaking = true;
        }
        else if(Input.GetKeyUp(BrakeKey))
        {
            isBreaking = false;
        }


    }

    void Look()
    {
        if(input_LookDir != Vector3.zero)
        {
            var relativeRot = (transform.position + input_LookDir) - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativeRot,Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
      
    }
    void Move()
    {
        //Check if Accelerating
        switch(isAccelerating)
        {
            case true:
             if(input_LookDir.magnitude >= 0.01 && (curMoveSpeed < m_maxSprintSpeed) && !isBreaking)
            {
                curMoveSpeed += m_accelerationSpeed * 1.5f * Time.deltaTime;
            }
            break;
            
            case false:
            if(input_LookDir.magnitude >= 0.01 && (curMoveSpeed < m_maxMoveSpeed) && !isBreaking)
            {
                curMoveSpeed += m_accelerationSpeed * Time.deltaTime;
            }
            break;
        }
        if(isBreaking)
        {
            if(curMoveSpeed > 0)
            {
                curMoveSpeed -= m_breakSpeed * Time.deltaTime;
            }
        }
        //Lose Speed
        if(input_LookDir.magnitude == 0 && curMoveSpeed > 0)
        {
             curMoveSpeed -= Time.deltaTime;
        }
        else if (!isAccelerating && curMoveSpeed > m_maxMoveSpeed)
        {
            curMoveSpeed -= m_breakSpeed * Time.deltaTime;
        }
        
        //MovePosition
        rigidBody.MovePosition(transform.position + (transform.forward) * curMoveSpeed * Time.deltaTime);
    }

    public void SetInputActive(bool _active)
    {
        isInputEnabled = _active;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
         Move();
    }

    // Update is called once per frame
    void Update()
    {
        if(isInputEnabled)
        {
            ProcessInput();
            Look();
        }
       
       
    }
}
