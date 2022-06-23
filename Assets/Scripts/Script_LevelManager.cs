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

    [SerializeField] GameObject EnemyA;
    [SerializeField] GameObject EnemyB;
    [SerializeField] GameObject EnemyC;
    [SerializeField] GameObject EnemyD;

    public float CurentBounty;

    [SerializeField] int EnemyASpawnRate;
    [SerializeField] int EnemyBSpawnRate;
    [SerializeField] int EnemyCSpawnRate;
    [SerializeField] int EnemyDSpawnRate;

    public 

    // Start is called before the first frame update
    void Awake()
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

        CurentBounty = Script_PlayerStatManager.Instance.Bounty;

        if (CurentBounty >= 0 && CurentBounty < 10)
        {
            EnemyASpawnRate = 10;
            EnemyBSpawnRate = 5;
            EnemyCSpawnRate = 0;
            EnemyDSpawnRate = 0;
        }

        if (CurentBounty >= 10 && CurentBounty < 20)
        {
            EnemyASpawnRate = 10;
            EnemyBSpawnRate = 10;
            EnemyCSpawnRate = 2;
            EnemyDSpawnRate = 0;
        }

        if (CurentBounty >= 20 && CurentBounty < 30)
        {
            EnemyASpawnRate = 5;
            EnemyBSpawnRate = 10;
            EnemyCSpawnRate = 5;
            EnemyDSpawnRate = 2;
        }

        if (CurentBounty >= 30 && CurentBounty < 40)
        {
            EnemyASpawnRate = 5;
            EnemyBSpawnRate = 10;
            EnemyCSpawnRate = 2;
            EnemyDSpawnRate = 10;
        }


        int SumSpawnRates = EnemyASpawnRate + EnemyBSpawnRate + EnemyCSpawnRate + EnemyDSpawnRate;

        Debug.Log(SumSpawnRates);

        foreach (var Pos in EnemySpawnPos)
        {
            int EnemyToSpawn = (int)Random.Range(0,SumSpawnRates);

             Debug.Log(EnemyToSpawn);

            if (EnemyToSpawn >= 0  && EnemyToSpawn < EnemyASpawnRate)
            {
                Pos.GetComponent<Scr_EnemySpawnPoint>().Enemy = EnemyA;
            }
            else if (EnemyToSpawn >= EnemyASpawnRate  && EnemyToSpawn <EnemyASpawnRate + EnemyBSpawnRate)
            {
                Pos.GetComponent<Scr_EnemySpawnPoint>().Enemy = EnemyB;
            }
            else if (EnemyToSpawn >= EnemyASpawnRate +  EnemyBSpawnRate  && EnemyToSpawn < EnemyASpawnRate + EnemyBSpawnRate + EnemyCSpawnRate)
            {
                Pos.GetComponent<Scr_EnemySpawnPoint>().Enemy = EnemyC;
            }
            else if (EnemyToSpawn >= EnemyASpawnRate + EnemyBSpawnRate + EnemyCSpawnRate  && EnemyToSpawn < EnemyASpawnRate + EnemyBSpawnRate + EnemyCSpawnRate + EnemyDSpawnRate)
            {
                Pos.GetComponent<Scr_EnemySpawnPoint>().Enemy = EnemyD;
            }
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
        LevelSections[0].isStatic = true;

        for (int i = 1; i < LevelSections.Length; i++)
        {
            LevelSections[i].transform.localPosition = LevelSections[i-1].transform.GetChild(0).transform.position; 
            LevelSections[i].transform.localRotation = LevelSections[i-1].transform.GetChild(0).transform.rotation;
            LevelSections[i].isStatic = true;
        } 

        EndRoom.transform.localPosition = LevelSections[LevelSections.Length - 1].transform.GetChild(0).transform.position; 
        EndRoom.transform.localRotation = LevelSections[LevelSections.Length - 1].transform.GetChild(0).transform.rotation;
        EndRoom.isStatic = true;
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
