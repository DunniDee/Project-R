using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Tram : MonoBehaviour
{
    public float Speed = 5.0f;
    public bool isMoving = false;

    public enum MoveAlignment { 
        X,
        Y,
        Z
    }

    public MoveAlignment CurrentMovementAlignment = MoveAlignment.Z;

    public Vector3 StopPosition = Vector3.zero;

    public Vector3 MoveDirection = Vector3.zero;

    [SerializeField] Vector3 Velocity = Vector3.zero;
    Scr_PlayerMotor motorReference;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(motorReference == null) motorReference = other.GetComponent<Scr_PlayerMotor>();
            switch (CurrentMovementAlignment)
            {
                case MoveAlignment.X:
                    motorReference.m_TertiaryVelocity = new Vector3(Velocity.x, 0, 0);
                    break;
                // Stop Along the Y Axis
                case MoveAlignment.Y:
                    motorReference.m_TertiaryVelocity = new Vector3(0, Velocity.y, 0);
                    break;
                // Stop Along the Z Axis
                case MoveAlignment.Z:
                    motorReference.m_TertiaryVelocity = new Vector3(0, 0, Velocity.z);
                    break;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isMoving && other.CompareTag("Player"))
        {
            switch (CurrentMovementAlignment)
            {
                case MoveAlignment.X:
                    motorReference.m_TertiaryVelocity = new Vector3(Velocity.x, 0, 0);
                    break;
                // Stop Along the Y Axis
                case MoveAlignment.Y:
                    motorReference.m_TertiaryVelocity = new Vector3(0, Velocity.y, 0);
                    break;
                // Stop Along the Z Axis
                case MoveAlignment.Z:
                    motorReference.m_TertiaryVelocity = new Vector3(0, 0, Velocity.z);
                    break;
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (motorReference == null) motorReference = other.GetComponent<Scr_PlayerMotor>();
            motorReference.m_TertiaryVelocity = Vector3.zero;
            switch (CurrentMovementAlignment)
            {
                case MoveAlignment.X:
                    motorReference.m_MomentumDirection += new Vector3(Velocity.x, 0, 0);
                    break;
                // Stop Along the Y Axis
                case MoveAlignment.Y:
                    motorReference.m_MomentumDirection += new Vector3(0, Velocity.y, 0);
                    break;
                // Stop Along the Z Axis
                case MoveAlignment.Z:
                    motorReference.m_MomentumDirection += new Vector3(0, 0, Velocity.z);
                    break;
            }
           
        }
    }

    public void SetIsMoving(bool _b)
    {
        isMoving = _b;
    }
    public void OnValidate()
    {
        switch (CurrentMovementAlignment)
        {
            case MoveAlignment.X:
                MoveDirection = new Vector3(1, 0, 0);
                break;
            // Stop Along the Y Axis
            case MoveAlignment.Y:
                MoveDirection = new Vector3(0, 1, 0);
                break;
            // Stop Along the Z Axis
            case MoveAlignment.Z:
                MoveDirection = new Vector3(0, 0, 1);
                break;
        }
    }

    public void Move()
    {
        switch (CurrentMovementAlignment)
        {
            //Stop along the X Axis
            case MoveAlignment.X:
                Velocity = MoveDirection * (Speed);
                transform.position += Velocity * Time.deltaTime;
                if (transform.position.x >= StopPosition.x)
                {
                    isMoving = false;
                    Velocity = Vector3.zero;
                }
                break;
            // Stop Along the Y Axis
            case MoveAlignment.Y:
                Velocity = MoveDirection * (Speed);
                transform.position += Velocity * Time.deltaTime;
                if (transform.position.y >= StopPosition.y)
                {
                    isMoving = false;
                    Velocity = Vector3.zero;
                }
                break;

            // Stop Along the Z Axis
            case MoveAlignment.Z:
                Velocity = MoveDirection * (Speed);
                transform.position += Velocity * Time.deltaTime;
                if (transform.position.z >= StopPosition.z)
                {
                    isMoving = false;
                    Velocity = Vector3.zero;
                }
                break;
        }

       
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            Move();
        }
    }

    private void OnDrawGizmos()
    {
        switch (CurrentMovementAlignment)
        {
            case MoveAlignment.X:
                StopPosition.z = transform.position.z;
                StopPosition.y = transform.position.y;
                break;
            case MoveAlignment.Y:
                StopPosition.x = transform.position.x;
                StopPosition.z = transform.position.z;
                break;
            case MoveAlignment.Z:
                StopPosition.x = transform.position.x;
                StopPosition.y = transform.position.y;
                break;
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, StopPosition);
        Gizmos.DrawSphere(StopPosition, 2.0f);
    }
}
