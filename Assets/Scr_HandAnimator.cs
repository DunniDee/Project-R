using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_HandAnimator : MonoBehaviour
{
    [SerializeField] Scr_CameraEffects CameraEffects;

    // Update is called once per frame
    void Update()
    {

        transform.localRotation = Quaternion.Lerp(transform.localRotation,Quaternion.Euler(CameraEffects.RotateTo), Time.deltaTime * 5);
    }
}
