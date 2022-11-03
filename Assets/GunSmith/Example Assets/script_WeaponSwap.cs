using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_WeaponSwap : MonoBehaviour
{
    //Singleton Pattern
    public static script_WeaponSwap Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        InitalseEquippedWeapons();

       
        for (int i = 0; i < Weapons.Length; i++)
        {
            Weapons[i].GetComponent<Script_WeaponBase>().weaponListIndex = i;
        }
        LoadEquipmentLoadout();

        EquippedWeapons[EquippedIndex].SetActive(true);
    }

    // Start is called before the first frame update
    public GameObject[] Weapons;
     public List<GameObject> EquippedWeapons;
    [SerializeField] Animator[] WeaponAnimations;
    [SerializeField] KeyCode weapon1_Input = KeyCode.Alpha1;
    [SerializeField] KeyCode weapon2_Input = KeyCode.Alpha2;

    [SerializeField] public int EquippedIndex = 0;
    int m_LastIndex = 0;
    bool m_IsActive = true;

    public int MaxEquippedWeapons = 2;

    public void SaveEquipmentLoadout()
    {
        string weaponIndexer = "weapon";
        
        for (int i = 0; i < EquippedWeapons.Count; i++)
        {
            Script_WeaponBase curWeapon = EquippedWeapons[i].GetComponent<Script_WeaponBase>();
            PlayerPrefs.SetInt(weaponIndexer + i.ToString(), curWeapon.weaponListIndex);
            Debug.Log(curWeapon.name + "|" + curWeapon.weaponListIndex.ToString() + " Saved");
        }
    }

    public void LoadEquipmentLoadout()
    {
        for (int i = 0; i < MaxEquippedWeapons; i++)
        {
            EquippedWeapons[i] = Weapons[PlayerPrefs.GetInt("weapon" + i.ToString())];
            Debug.Log(EquippedWeapons[i].name + "|" + EquippedWeapons[i].GetComponent<Script_WeaponBase>().weaponListIndex.ToString() + " Loaded");
        }
    }

    public void SetCanShoot(bool _b)
    {
        EquippedWeapons[EquippedIndex].GetComponent<Script_WeaponBase>().enabled = _b;
        m_IsActive = _b;
    }

  
    public void Start()
    {
        m_LastIndex = EquippedIndex;


      

       
        

    }

    private void InitalseEquippedWeapons()
    {
        int i = 0;

        foreach (GameObject Weapon in Weapons)
        {
            if (i >= MaxEquippedWeapons)
            {
                return;
            }
            else
            {
                Script_WeaponBase weaponBase = Weapon.GetComponent<Script_WeaponBase>();

                EquippedWeapons.Add(Weapon);

                Weapon.SetActive(false);
            }
           
            i++;
        }

    }

   
    void Update()
    {

        if (m_IsActive)
        {
            if (m_LastIndex != EquippedIndex)
            {
                m_LastIndex = EquippedIndex;

                foreach (var Weapon in Weapons)
                {
                    Weapon.SetActive(false);
                }

                EquippedWeapons[EquippedIndex].SetActive(true);
            }

            if (Input.GetKeyDown(weapon1_Input))
            {
                EquippedIndex = 0;
            }
            else if (Input.GetKeyDown(weapon2_Input))
            {
                EquippedIndex = 1;
            }

            if (Input.mouseScrollDelta.y >= 1)
            {
                EquippedIndex++;
                if (EquippedIndex > EquippedWeapons.Count - 1)
                {
                    EquippedIndex = 0;
                }
            }
            else if (Input.mouseScrollDelta.y <= -1)
            {
                EquippedIndex++;
                if (EquippedIndex > EquippedWeapons.Count - 1)
                {
                    EquippedIndex = 0;
                }
            }
        }
        
    }

    public void JumpAnim()
    {
        WeaponAnimations[EquippedIndex].SetTrigger("jump");
    }

    public void SlideAnim(bool _isSliding)
    {
        WeaponAnimations[EquippedIndex].SetBool("sliding", _isSliding);
        if (_isSliding ) 
        {
            WeaponAnimations[EquippedIndex].SetTrigger("startslide");
        }
        else
        {
            WeaponAnimations[EquippedIndex].SetTrigger("endslide");
        }
    }

    public void DashAnim()
    {
        WeaponAnimations[EquippedIndex].SetTrigger("dash");
    }

    public void SetActiveAnim(bool _isActive)
    {
        Debug.Log("Setting anim to " + _isActive);
        WeaponAnimations[EquippedWeapons[EquippedIndex].GetComponent<Script_WeaponBase>().weaponListIndex].SetBool("hide", _isActive);

        if (_isActive)
        {
            WeaponAnimations[EquippedIndex].gameObject.GetComponent<Script_WeaponBase>().ShotTimer = 1;
        }
    }
}

