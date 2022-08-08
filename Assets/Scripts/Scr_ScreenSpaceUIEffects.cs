using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_ScreenSpaceUIEffects : MonoBehaviour
{
    [SerializeField] Scr_CameraEffects CamEffects;
    [SerializeField] RectTransform Rect;
    Vector3 LerpToPos;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        LerpToPos = Vector3.Lerp(LerpToPos, new Vector3(CamEffects.RotateTo.y - CamEffects.RotateTo.z, -CamEffects.RotateTo.x,0), Time.deltaTime * 10);
        Rect.anchoredPosition = LerpToPos;
    }
}
