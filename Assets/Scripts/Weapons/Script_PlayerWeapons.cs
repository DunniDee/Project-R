using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerWeapons : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Script_WeaponBase Primary;
    [SerializeField] Script_WeaponBase Secondary;
    [SerializeField] Script_WeaponBase Melee;

    [SerializeField] GameObject PrimaryModel;
    [SerializeField] GameObject SecondaryModel;
    [SerializeField] GameObject MeleeModel;

    [SerializeField] KeyCode PrimaryKey;
    [SerializeField] KeyCode SecondaryKey;
    [SerializeField] KeyCode MeleeKey;

    float EquipTimer = 0;

    enum Equip
    {
        Primary,
        Secondary,
        Melee,
    }

    [SerializeField] Equip CurrentEquip = Equip.Primary;

    private void Start() 
    {
        EquipPrimary();
    }

    private void Update() 
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            ChangeEquip(1);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            ChangeEquip(-1);       
        }

        if (Input.GetKeyDown(PrimaryKey))
        {
            EquipPrimary();
        }

        if (Input.GetKeyDown(SecondaryKey))
        {
            EquipSecondary();
        }

        if (Input.GetKeyDown(MeleeKey))
        {
            EquipMeele();
        }

        if (EquipTimer > 0)
        {
            EquipTimer-= Time.deltaTime;
        }
        else
        {
            switch (CurrentEquip)
            {
                case Equip.Primary:
                    Primary.enabled = true;
                break;

                case Equip.Secondary:
                    Secondary.enabled = true;
                break;

                case Equip.Melee:
                    Melee.enabled = true;
                break;

                default:
                    Debug.LogWarning("Weapon Swap Sqitch verry broken :^(");
                break;
            }
        }
    }

    void ChangeEquip(int delta)
    {
        int Temp = (int)CurrentEquip + delta;

        if (Temp > 2)
        {
            Temp = 0;
        }
        if (Temp < 0)
        {
            Temp = 2;
        }

        CurrentEquip = (Equip)Temp;

        switch (CurrentEquip)
        {
            case Equip.Primary:
                EquipPrimary();
            break;

            case Equip.Secondary:
                EquipSecondary();
            break;

            case Equip.Melee:
                EquipMeele();
            break;

            default:
                Debug.LogWarning("Weapon Swap Sqitch verry broken :^(");
            break;
        }
    }

    void EquipPrimary()
    {
        // Primary.enabled = true;
        PrimaryModel.SetActive(true);

        Secondary.enabled = false;
        SecondaryModel.SetActive(false);

        Melee.enabled = false;
        MeleeModel.SetActive(false);

        EquipTimer = 1;
    }

    void EquipSecondary()
    {
        //Secondary.enabled = true;
        SecondaryModel.SetActive(true);

        Melee.enabled = false;
        MeleeModel.SetActive(false);

        Primary.enabled = false;
        PrimaryModel.SetActive(false);

        EquipTimer = 1;
    }

    void EquipMeele()
    {
        //Melee.enabled = true;
        MeleeModel.SetActive(true);

        Primary.enabled = false;
        PrimaryModel.SetActive(false);

        Secondary.enabled = false;
        SecondaryModel.SetActive(false);

        EquipTimer = 1;
    }
}
