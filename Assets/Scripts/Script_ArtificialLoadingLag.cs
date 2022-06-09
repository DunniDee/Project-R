using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ArtificialLoadingLag : MonoBehaviour
{
    [SerializeField] float LagTime;
 private void Awake() 
 {
     for (int i = 0; i < 1000; i++)
     {
         LagTime *= LagTime;
         LagTime = Mathf.Sqrt(LagTime);
     }
 }
}
