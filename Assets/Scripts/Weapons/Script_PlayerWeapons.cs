using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_PlayerWeapons : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Script_WeaponBase Primary;
    [SerializeField] Script_WeaponBase Secondary;

    [SerializeField] GameObject PrimaryModel;
    [SerializeField] GameObject SecondaryModel;

    [SerializeField] KeyCode PrimaryKey;
    [SerializeField] KeyCode SecondaryKey;

    [SerializeField] Animator Anim;

    // [SerializeField] bool Is
}
