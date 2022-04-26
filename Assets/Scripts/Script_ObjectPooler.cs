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
        public int CurrentSize;
        public int MaxSize;
    }

    #region Singleton
    public static Script_ObjectPooler Instance;
    private void Awake() 
    {
        Instance = this;
    }
    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            Queue<GameObject> ObjectPool = new Queue<GameObject>();

            // for (int i = 0; i < pool.size; i++)
            // {
            //     GameObject obj = Instantiate(pool.prefab);
            //     obj.SetActive(false);
            //     ObjectPool.Enqueue(obj);
            // }

            poolDictionary.Add(pool.tag, ObjectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag" + tag + " doesn't exsist");
            return null;
        }

        if (pools[0].CurrentSize < pools[0].MaxSize )
        {
            GameObject obj = Instantiate(pools[0].prefab);
            obj.SetActive(false);
            poolDictionary[tag].Enqueue(obj);
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }


}
