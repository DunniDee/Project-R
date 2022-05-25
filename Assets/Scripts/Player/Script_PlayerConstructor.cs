using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerConstructor : MonoBehaviour
{
    [SerializeField] GameObject PlayerPrefab;
    [SerializeField] GameObject Player;
    private void Awake() 
    {
        Player = Instantiate(PlayerPrefab,Vector3.up, Quaternion.identity);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
