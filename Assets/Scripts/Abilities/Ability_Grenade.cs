using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Grenade : Script_AbilityBase
{
    [SerializeField] float Force;
    [SerializeField] float Damage;
    [SerializeField] float Radius;
    [SerializeField] GameObject Grenade;
    Transform Look;
    [SerializeField] Transform ShotPos;
    [SerializeField] Transform SeekPos;
    [SerializeField] Animator Anim;


    protected override void OnAbilityStart()
    {
        base.OnAbilityStart();
         Invoke("Shoot", 1);
        Anim.SetBool("Deployed", true);
    }

    void Shoot()
    {
        GameObject Proj = Instantiate(Grenade);

        Proj.transform.position = ShotPos.position;
        Proj.transform.rotation = ShotPos.rotation;
        Proj.GetComponent<Rigidbody>().AddForce(Force * ShotPos.forward, ForceMode.VelocityChange);

        Proj.GetComponent<Scr_TPGrenade>().SetPlayer(gameObject);

        Anim.SetTrigger("Shoot");
        Anim.SetBool("Deployed", false);
    }
}
