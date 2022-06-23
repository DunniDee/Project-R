using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Salvo : Script_AbilityBase
{
    [SerializeField] int BurstCount;
    [SerializeField] float BurstSpeed;
    [SerializeField] float Force;
    [SerializeField] float Damage;
    [SerializeField] float Radius;
    [SerializeField] GameObject Grenade;
    Transform Look;
    [SerializeField] Transform ShotPos;
    [SerializeField] Transform SeekPos;

    [SerializeField] Animator Anim;

    [SerializeField] AudioSource AS;

    [SerializeField] AudioClip UseAbility;

    [SerializeField] AudioClip[] Clips;
    private void Start()
    {
        Look = transform.GetChild(0);
    }

    protected override void OnAbilityStart()
    {
        base.OnAbilityStart();
         Invoke("Salvo", 1);
        Anim.SetBool("Deployed", true);
        PlaySound();
    }

    protected override void CustomUpdate()
    {
        base.CustomUpdate();
        
        RaycastHit hit;
        if (Physics.Raycast(Look.position, Look.forward, out hit,500))
        {
            SeekPos.position = hit.point;
        }
        else
        {
            SeekPos.position = transform.position + Look.forward * 500;
        }
    }

    void Salvo()
    {
        for (int i = 0; i < BurstCount; i++)
        {
            Invoke("Shoot", BurstSpeed * i);
        }
        Anim.SetBool("Deployed", false);
    }

    void Shoot()
    {
        GameObject Proj = ObjectPooler.Instance.GetObject(Grenade);

        Proj.transform.position = ShotPos.position;
        Proj.transform.rotation = ShotPos.rotation;

        Scr_SalvoMissile SpTemp = Proj.GetComponent<Scr_SalvoMissile>();
        SpTemp.SetDamage(Damage);
        SpTemp.SetSpeed(Force);
        SpTemp.SetRadius(Radius);
        SpTemp.SetSeekTransform(SeekPos);
        SpTemp.SetlifeTime(5);

        Anim.SetTrigger("Shoot");

        AS.PlayOneShot(UseAbility);
    }

    void PlaySound()
    {
        AS.PlayOneShot(Clips[Random.Range(0,Clips.Length)]);
        
    }
}
