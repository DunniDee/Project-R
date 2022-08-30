using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Tram : MonoBehaviour
{
    public float Speed = 5.0f;
    public bool isMoving = false;

    public Vector3 StopPosition = Vector3.zero;

    public Vector3 MoveDirection = Vector3.zero;

    [SerializeField] Vector3 Velocity = Vector3.zero;
    Scr_PlayerMotor motorReference;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(motorReference == null) motorReference = other.GetComponent<Scr_PlayerMotor>();
            motorReference.m_TertiaryVelocity = new Vector3(0, 0, Velocity.z);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isMoving && other.CompareTag("Player"))
        {
            motorReference.m_TertiaryVelocity = new Vector3(0,0,Velocity.z);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (motorReference == null) motorReference = other.GetComponent<Scr_PlayerMotor>();
            motorReference.m_TertiaryVelocity = Vector3.zero;
            motorReference.m_MomentumDirection += new Vector3(0, 0, Velocity.z);
        }
    }

    public void SetIsMoving(bool _b)
    {
        isMoving = _b;
    }
    public void Move()
    {
        Velocity = MoveDirection * (Speed);
        transform.position += Velocity * Time.deltaTime;
        if (transform.position.z >= StopPosition.z)
        {
            isMoving = false;
            Velocity = Vector3.zero;
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
        MoveDirection = transform.forward;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, StopPosition);
        Gizmos.DrawSphere(StopPosition, 2.0f);
    }
}
