using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_HandAnimator : MonoBehaviour
{
    [SerializeField] Scr_CameraEffects CameraEffects;
    [SerializeField] Scr_HandAnimator HandEffects;
    public Vector3 RotateTo;
    // Update is called once per frame
    void Update()
    {
        RotateTo = Vector3.Lerp(RotateTo,CameraEffects.RotateTo, Time.deltaTime * 5);

        transform.localRotation = Quaternion.Euler(RotateTo);
    }
}
