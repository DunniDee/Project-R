using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerStatManager : MonoBehaviour
{
    public struct Stat
    {
        public Stat(float _stat)
        {
            s_default = _stat;
            s_Modified = _stat;
        }
        public float s_default { get; set; }
        public float s_Modified { get; set; }
    }
    // Start is called before the first frame update


    [SerializeField] Stat Firerate;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
