using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_LootManager : MonoBehaviour
{
    #region singleton
    public static Scr_LootManager Instance;

    public Grid Inventorygrid;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("Lootmanager fucked up");
            Destroy(Instance);
        }
    }
    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
