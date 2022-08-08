using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Scr_PlayerRespawner : MonoBehaviour
{
    public static Scr_PlayerRespawner Instance;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (FindObjectOfType<Scr_PlayerMotor>() == null)
            { 
                //Instantiate(PlayerPrefab, )
            }
            DontDestroyOnLoad(gameObject);
        }
        else {
            Debug.Log("Playerrespawner fucked up");
            Destroy(gameObject);
        }
    }
    public GameObject PlayerPrefab;
    Scr_PlayerHealth playerHealth;
    [SerializeField] Transform playerTransform;
    public List<GameObject> RespawnPoints;

    private void Init()
    { 
        //PlayerPrefab = Resources.Load(")
    }
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = FindObjectOfType<Scr_PlayerHealth>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth.currentHealth <= 0)
        {
            
            playerTransform.position = RespawnPoints[0].transform.position;
            playerHealth.ResetHealth();
        }
    }
}
