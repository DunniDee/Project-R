using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scr_DiegeticHUD : MonoBehaviour
{
    public Scr_PlayerHealth playerhealth;
    [SerializeField] Transform OuterTransform;
    [SerializeField] Transform MiddleTransform;
    [SerializeField] Transform InnerTransform;
    [SerializeField] Transform AmmoCountTransform;
    [SerializeField] Transform HealthCountTransform;
    public int AmmoReserve;
    public int MagSize;
    public int AmmoCount;

    public float Health;
    [SerializeField] TextMeshPro AmmoText;
    [SerializeField] TextMeshPro AmmoReserveText;
    [SerializeField] TextMeshPro WeaponText;
    [SerializeField] TextMeshPro HealthText;
    [SerializeField] MeshRenderer AmmoArcMesh;
    [SerializeField] MeshRenderer HealthArcMesh;

    [SerializeField] TextMeshPro AltFireText;

    float AmmoCountAngleLerp;
    float AmmoCountAngle;

    float HealthCountAngleLerp;
    float HealthCountAngle;

    [SerializeField] Color BaseColor;
    [SerializeField] Color MidColor;
    [SerializeField] Color LowColor;

    float AmmoAngleLerp;
    float HealthAngleLerp;

    Color AmmoColorLerp = Color.magenta;
    Color HealthColorLerp = Color.magenta;


    public bool Toggle = false;
    // Start is called before the first frame update
    void Start()
    {
        playerhealth = GetComponentInParent<Scr_PlayerHealth>();
        OuterTransform.localScale = Vector3.zero;
        MiddleTransform.localScale = Vector3.zero;
        InnerTransform.localScale = Vector3.zero;

        Invoke("ToggleHud", 0.25f);
        AmmoArcMesh.material.EnableKeyword("_Color");
        HealthArcMesh.material.EnableKeyword("_Color");
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
        AmmoReserveText.text = AmmoReserve.ToString();
        HealthText.text = ((int)playerhealth.currentHealth).ToString();

        AmmoAngleLerp = (float)AmmoCount/(float)MagSize;
        HealthAngleLerp = playerhealth.currentHealth / playerhealth.maxHealth;

        AmmoColorLerp = Color.magenta;
        HealthColorLerp = Color.magenta;

        if(AmmoAngleLerp > 0.5f)
        {
            AmmoColorLerp = Color.Lerp(MidColor,BaseColor,(AmmoAngleLerp - 0.5f)* 2);
        }
        else
        {
            AmmoColorLerp = Color.Lerp(LowColor,MidColor,AmmoAngleLerp * 2);  
        }
        AmmoText.color = AmmoColorLerp;  
        AmmoArcMesh.material.SetColor("_Color", AmmoColorLerp);

        if(HealthAngleLerp > 0.5f)
        {
            HealthColorLerp = Color.Lerp(MidColor,BaseColor,(HealthAngleLerp - 0.5f)* 2);
        }
        else
        {
            HealthColorLerp = Color.Lerp(LowColor,MidColor,HealthAngleLerp * 2);  
        }

        HealthText.color = HealthColorLerp;  
        HealthArcMesh.material.SetColor("_Color", HealthColorLerp);

        AmmoCountAngleLerp = Mathf.Lerp(-45,-77.5f, AmmoAngleLerp);
        AmmoCountAngle = Mathf.Lerp(AmmoCountAngle,AmmoCountAngleLerp, Time.deltaTime * 5);

        HealthCountAngleLerp = Mathf.Lerp(103,135 , HealthAngleLerp);
        HealthCountAngle = Mathf.Lerp(HealthCountAngle,HealthCountAngleLerp, Time.deltaTime * 5);
        
        AmmoCountTransform.localRotation = Quaternion.Euler(0,0,AmmoCountAngle);
        HealthCountTransform.localRotation = Quaternion.Euler(0,0,HealthCountAngle);
    }

    void ToggleHud()
    {
        Toggle = !Toggle;
    }

    public void SetGunName(string _GunName)
    {
        WeaponText.text = _GunName;
    }

    public void SetAltFireName(string _AltFire)
    {
        AltFireText.text = _AltFire;
    }
}
