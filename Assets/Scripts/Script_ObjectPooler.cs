using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        
        public string tag;
        public GameObject prefab;
        public int size;
    }

    
    public List<Pool> Pools;
    public Dictionary<string, Queue<GameObject>> PoolDictionary;

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
