using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_DamagePopupManager : MonoBehaviour
{
    #region singleton
    public static Scr_DamagePopupManager Instance;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Damagepopup Manager fucked up");
            Destroy(gameObject);
        }
    }
    #endregion

    [Header("Damagepopup Properties")]
    [SerializeField]
    private GameObject damagePopupPrefab;

    [SerializeField]
    private GameObject HealthOrbPrefab;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_DamageAmount"></param>
    /// <param name="_PopupParent"></param>
    public void DisplayDamagePopup(int _DamageAmount, Transform _PopupParent)
    {
        var obj = Instantiate(damagePopupPrefab, _PopupParent.transform.position, Quaternion.identity, _PopupParent);
        obj.GetComponent<Scr_DamagePopup>().Init(_DamageAmount);
    }


    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_HealthOrbParent"></param>
    public void CreateHealthOrb(Transform _HealthOrbParent)
    {
        var obj = Instantiate(HealthOrbPrefab, _HealthOrbParent.transform.position, Quaternion.identity);
       
    }

}
