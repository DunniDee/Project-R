using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_HandAnimator : MonoBehaviour
{
    [SerializeField] Scr_CameraEffects CameraEffects;
    [SerializeField] Scr_HandAnimator HandEffects;
    public Vector3 RotateTo;
    public bool rotateZ;
    // Update is called once per frame
    void Update()
    {
        if (rotateZ)
        {
            RotateTo = HandEffects.RotateTo;
            RotateTo.x = 0;
            RotateTo.y = 0;
            RotateTo.z = -RotateTo.y * 100;
        }
        else
        {
            RotateTo = Vector3.Lerp(RotateTo,CameraEffects.RotateTo, Time.deltaTime * 5);
        }
        transform.localRotation = Quaternion.Euler(RotateTo);
    }
}
