using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class Script_LevelManager : MonoBehaviour
{
    [SerializeField] GameObject[] LevelSections;
    [SerializeField] GameObject EndRoom;
    [SerializeField] GameObject[] EnemySpawnPos;
    [SerializeField] GameObject[] EnemyList;
    [SerializeField] NavMeshSurface[] surfaces;

    [SerializeField] GameObject DummyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        GetSpawnPoints();

        Shuffle();
        Arrange();
        RebakeNavMesh();

        SpawnEnemies();
    }

    void GetSpawnPoints()
    {
        EnemySpawnPos = GameObject.FindGameObjectsWithTag("EnemySpawnPos");
    }

    void SpawnEnemies()
    {
        foreach (var Pos in EnemySpawnPos)
        {
            ObjectPooler.Instance.GetObject(DummyPrefab);
            DummyPrefab.transform.position = Pos.transform.position;
            DummyPrefab.transform.rotation = Pos.transform.rotation;
        }
    }

    void Shuffle()
    {
        for (int i = 0; i < LevelSections.Length; i++)
        {
            int a = Random.Range(0,LevelSections.Length);
            int b = Random.Range(0,LevelSections.Length);


            GameObject temp = LevelSections[a];
            LevelSections[a] = LevelSections[b];
            LevelSections[b] = temp;
        } 
    }

    void Arrange()
    {
        LevelSections[0].transform.localPosition = Vector3.zero;
        LevelSections[0].transform.localRotation = Quaternion.identity;

        for (int i = 1; i < LevelSections.Length; i++)
        {
            LevelSections[i].transform.localPosition = LevelSections[i-1].transform.GetChild(0).transform.position; 
            LevelSections[i].transform.localRotation = LevelSections[i-1].transform.GetChild(0).transform.rotation;
        } 

        EndRoom.transform.localPosition = LevelSections[LevelSections.Length - 1].transform.GetChild(0).transform.position; 
        EndRoom.transform.localRotation = LevelSections[LevelSections.Length - 1].transform.GetChild(0).transform.rotation;
    }

    void SetStatic()
    {
        for (int i = 0; i < LevelSections.Length; i++)
        {
            LevelSections[i].isStatic = true;
        } 
    }

    void RebakeNavMesh()
    {
        for (int i = 0; i < surfaces.Length; i++) 
        {
            surfaces [i].BuildNavMesh ();    
        }   
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Shuffle();
            Arrange();
            RebakeNavMesh();
        }
    }
}
