using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilit_Deflect : Scr_AltFireBase
{
    bool IsDeflecting = false;
    [SerializeField] Transform Look;
    [SerializeField] Scr_CameraEffects CamEffects;

    [SerializeField] AudioSource AS;

    [SerializeField] AudioClip[] DeflectSounds;

    [SerializeField] Animator Anim;
 
    protected override void OnAbilityStart()
    {
        base.OnAbilityStart();
        IsDeflecting = true;
        Anim.SetBool("IsDeflecting",IsDeflecting);
    }

    protected override void OnAbilityEnd()
    {
        base.OnAbilityEnd();
        IsDeflecting = false;
        Anim.SetBool("IsDeflecting",IsDeflecting);
    }

    private void OnTriggerEnter(Collider other) {
        if (IsDeflecting && other.CompareTag("Projectile"))
        {
            Rigidbody RB = other.GetComponent<Rigidbody>();
            RB.velocity = Look.forward * 100;
            CamEffects.Fov += 5;
            CamEffects.ShakeAmplitude = 0.5f;
            CamEffects.ShakeTime = 0.5f;

            CamEffects.RotateTo += new Vector3(Random.Range(-5,5),Random.Range(-5,5),Random.Range(-5,5));
            AS.PlayOneShot(DeflectSounds[Random.Range(0, DeflectSounds.Length -1)]);

            Anim.SetTrigger("Deflect");
        }
    }
}
