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
            DontDestroyOnLoad(gameObject);
        }

    }

    // Start is called before the first frame update
    [SerializeField] GameObject[] Weapons;
    [SerializeField] Animator[] WeaponAnimations;
    [SerializeField] KeyCode ScrollLeft = KeyCode.Q;
    [SerializeField] KeyCode ScrollRight = KeyCode.E;

    [SerializeField] public int Index = 0;
    int m_LastIndex = 0;

    // Update is called once per frame
    public void Start() 
    {
        FindObjectOfType<GridController>().enabled = true;
        m_LastIndex = Index;
        // Load Weapon Stats to Player Stat Manager

        int i = 0;
      
        foreach (var Weapon in Weapons)
        {
            Script_WeaponBase weaponBase = Weapon.GetComponent<Script_WeaponBase>();

            Script_PlayerStatManager.Instance.WeaponStatList.Add(new Script_PlayerStatManager.WeaponStats());
            Script_PlayerStatManager.Instance.SetWeaponStats(i, weaponBase);

          

            Weapon.SetActive(false);
           
            i++;
        }

        Weapons[Index].SetActive(true);
    }

    public void UpdateWeaponStats()
    {
        int i = 0;
        foreach (var Weapon in Weapons)
        {
            Script_WeaponBase weaponBase = Weapon.GetComponent<Script_WeaponBase>();
            weaponBase.SetDamage(Script_PlayerStatManager.Instance.WeaponStatList[i].Modified_Damage);
            weaponBase.SetFireRate(Script_PlayerStatManager.Instance.WeaponStatList[i].Modified_Firerate);
            i++;
        }
    }
    void Update()
    {
        if (m_LastIndex != Index)
        {
            m_LastIndex = Index;

            foreach (var Weapon in Weapons)
            {
                Weapon.SetActive(false);
            }

            Weapons[Index].SetActive(true);
        }

        if(Input.GetKeyDown(ScrollRight))
        {
            Index++;
            if (Index > Weapons.Length - 1)
            {
                Index = 0;
            }
        }

        if(Input.GetKeyDown(ScrollLeft))
        {
            Index--;
            if (Index < 1)
            {
                Index = Weapons.Length - 1;
            }
        }
    }

    public void JumpAnim()
    {
        WeaponAnimations[Index].SetTrigger("jump");
    }

    public void SlideAnim(bool _isSliding)
    {
        WeaponAnimations[Index].SetBool("sliding", _isSliding);
        if (_isSliding ) 
        {
            WeaponAnimations[Index].SetTrigger("startslide");
        }
        else
        {
            WeaponAnimations[Index].SetTrigger("endslide");
        }
    }

    public void DashAnim()
    {
        WeaponAnimations[Index].SetTrigger("dash");
    }

    public void SetActiveAnim(bool _isActive)
    {
        WeaponAnimations[Index].SetBool("hide", _isActive);
    }
}

