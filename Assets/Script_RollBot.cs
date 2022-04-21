using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_RollBot : MonoBehaviour
{
    [SerializeField] Rigidbody RB;
    [SerializeField] float Speed;
    // Start is called before the first frame update
    void Start()
    {
        RB=gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        RB.AddTorque(Vector3.right * Speed * Time.deltaTime,ForceMode.Acceleration);
    }
}
