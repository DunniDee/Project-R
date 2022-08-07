using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scr_ScrollableButton : MonoBehaviour
{
    public TMP_Text tmpText;
    // Start is called before the first frame update
    void Start()
    {
        tmpText = GetComponentInChildren<TMP_Text>();
    }

}
