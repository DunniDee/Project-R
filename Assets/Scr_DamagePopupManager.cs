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

    [SerializeField]
    private GameObject damagePopupPrefab;

    public void DisplayDamagePopup(int _DamageAmount, Transform _PopupParent)
    {
        var obj = Instantiate(damagePopupPrefab, _PopupParent.transform.position, Quaternion.identity, _PopupParent);
        obj.GetComponent<Scr_DamagePopup>().Init(_DamageAmount);
    }
    
}
