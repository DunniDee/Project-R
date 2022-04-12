using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_WeaponSwap : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject[] Weapons;
    [SerializeField] KeyCode ScrollLeft = KeyCode.Q;
    [SerializeField] KeyCode ScrollRight = KeyCode.E;

    [SerializeField] int Index = 0;
    int m_LastIndex = 0;
    // Update is called once per frame

    private void Start() 
    {
        m_LastIndex = Index;

        foreach (var Weapon in Weapons)
        {
            Weapon.SetActive(false);
        }

        Weapons[Index].SetActive(true);
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
}
