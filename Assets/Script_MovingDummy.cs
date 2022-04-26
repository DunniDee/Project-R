using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_MovingDummy : MonoBehaviour
{
    [SerializeField] float Speed;
    Transform Dummy;
    Transform WaypointA;
    Transform WaypointB;
    bool IsMovingA = true;
    // Start is called before the first frame update
    void Start()
    {
        Dummy = transform.GetChild(0);
        WaypointA = transform.GetChild(1);
        WaypointB = transform.GetChild(2);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsMovingA)
        {
            if (Dummy.localPosition != WaypointA.localPosition)
            {
               Dummy.localPosition = Vector3.MoveTowards(Dummy.localPosition,WaypointA.localPosition, Speed * Time.deltaTime);
            }
            else
            {
                IsMovingA = false;
            }
        }
        else
        {
            if (Dummy.localPosition != WaypointB.localPosition)
            {
               Dummy.localPosition = Vector3.MoveTowards(Dummy.localPosition,WaypointB.localPosition, Time.deltaTime * Speed);
            }
            else
            {
                IsMovingA = true;
            }
        }
    }
}
