using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    //Singleton Pattern
    public static ObjectPooler Instance;
    private void Awake() 
    {
        Instance = this;
    }

    [SerializeField] private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>();

    public GameObject GetObject(GameObject gameObject)
    {
        if (objectPool.TryGetValue(gameObject.name, out Queue<GameObject> objectList))
        {
            if (objectList.Count == 0)
            {
                return CreateNewGameObject(gameObject);
            }
            else
            {
                GameObject _object = objectList.Dequeue();
                _object.SetActive(true);
                return _object;
            }
        }
        else
        {
            return CreateNewGameObject(gameObject);
        }
    }

        public GameObject GetObject(GameObject gameObject, Transform Parent)
    {
        if (objectPool.TryGetValue(gameObject.name, out Queue<GameObject> objectList))
        {
            if (objectList.Count == 0)
            {
                return CreateNewGameObject(gameObject);
            }
            else
            {
                GameObject _object = objectList.Dequeue();
                _object.transform.SetParent(Parent);
                _object.SetActive(true);
                return _object;
            }
        }
        else
        {
            return CreateNewGameObject(gameObject);
        }
    }

    private GameObject CreateNewGameObject(GameObject gameObject)
    {
        GameObject NewGO = Instantiate(gameObject);
        NewGO.name = gameObject.name;
        return NewGO;
    }

    public void ReturnObject(GameObject gameObject)
    {
        if (objectPool.TryGetValue(gameObject.name, out Queue<GameObject> objectList))
        {
            objectList.Enqueue(gameObject);
        }
        else
        {
            Queue<GameObject> newObjectQueue = new Queue<GameObject>();
            newObjectQueue.Enqueue(gameObject);
            objectPool.Add(gameObject.name, newObjectQueue);
        }

        gameObject.SetActive(false);
    }
}
