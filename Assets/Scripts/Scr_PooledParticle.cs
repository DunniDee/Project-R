using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_PooledParticle : MonoBehaviour
{
    [SerializeField] float Lifetime;
    // Update is called once per frame
    private void Start()
    {
        Invoke("Disable", Lifetime);
    }

    private void OnEnable() 
    {
        Invoke("Disable", Lifetime);
    }
    public void Disable()
    {
        ObjectPooler.Instance.ReturnObject(this.gameObject);
    }
}
