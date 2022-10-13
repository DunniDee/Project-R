 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class scr_ObjectiveHandler : MonoBehaviour
{
    public static scr_ObjectiveHandler i;

    public void Awake()
    {
        if (i == null)
        {
            i = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
    public Animator animator;
    [SerializeField] TMP_Text objectiveTextMesh;

    public void ShowObjective(string _ObjectiveText)
    {
        objectiveTextMesh.text = _ObjectiveText;
        animator.SetTrigger("Show");
    }

}
