using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scr_DiegeticHUD : MonoBehaviour
{
    [SerializeField] Transform OuterTransform;
    [SerializeField] Transform MiddleTransform;
    [SerializeField] Transform InnerTransform;
    [SerializeField] Transform AmmoCountTransform;
    [SerializeField] int AmmoReserve;
    [SerializeField] int MagSize;
    [SerializeField] int AmmoCount;
    [SerializeField] TextMeshPro AmmoText;

    float AmmoCountAngleLerp;
    float AmmoCountAngle;

    public bool Toggle = false;
    // Start is called before the first frame update
    void Start()
    {
        OuterTransform.localScale = Vector3.zero;
        MiddleTransform.localScale = Vector3.zero;
        InnerTransform.localScale = Vector3.zero;

        Invoke("ToggleHud", 0.25f);
    }
 
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ToggleHud();
        }

        if (Toggle)
        {
            OuterTransform.localScale = Vector3.Lerp(OuterTransform.localScale, Vector3.one, Time.deltaTime * 6);
            MiddleTransform.localScale = Vector3.Lerp(MiddleTransform.localScale, Vector3.one, Time.deltaTime * 5);
            InnerTransform.localScale = Vector3.Lerp(InnerTransform.localScale, Vector3.one, Time.deltaTime * 4);
        }
        else
        {
            OuterTransform.localScale = Vector3.Lerp(OuterTransform.localScale, Vector3.zero, Time.deltaTime * 4);
            MiddleTransform.localScale = Vector3.Lerp(MiddleTransform.localScale, Vector3.zero, Time.deltaTime * 5);
            InnerTransform.localScale = Vector3.Lerp(InnerTransform.localScale, Vector3.zero, Time.deltaTime * 6);
        } 
        AmmoText.text = AmmoCount.ToString();

        AmmoCountAngleLerp = Mathf.Lerp(-10,-77.5f, (float)AmmoCount/(float)MagSize);
        AmmoCountAngle = Mathf.Lerp(AmmoCountAngle,AmmoCountAngleLerp, Time.deltaTime * 5);
        
        AmmoCountTransform.localRotation = Quaternion.Euler(0,0,AmmoCountAngle);
    }

    void ToggleHud()
    {
        Toggle = !Toggle;
    }
}
