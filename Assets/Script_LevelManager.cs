using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_LevelManager : MonoBehaviour
{
    [SerializeField] GameObject[] LevelSections;
    // Start is called before the first frame update
    void Start()
    {
        Shuffle();
        Arrange();
        SetStatic();
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
    }

    void SetStatic()
    {
        for (int i = 0; i < LevelSections.Length; i++)
        {
            LevelSections[i].isStatic = true;
        } 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            Shuffle();
            Arrange();
            SetStatic();
        }
    }
}
