using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Melee : MonoBehaviour
{
    [SerializeField] KeyCode MeleeKey;
    [SerializeField] Animator Anim;
    [SerializeField] script_WeaponSwap Weapons;
    [SerializeField] float MeeleTime = 1.2f;
    float MeeleTimer = 0;
    [SerializeField] bool IsAttacking;
    [SerializeField] Scr_CameraEffects CameraEffects;

    [SerializeField] AudioClip[] MeeleSounds;
    [SerializeField] AudioSource AS;

    // Update is called once per frame
    void Update()
    {
        if (MeeleTimer > 0)
        {
            MeeleTimer -= Time.deltaTime;
            IsAttacking = true;
            Anim.transform.localRotation = Quaternion.Euler(0,0,0);
            Weapons.SetActiveAnim(true);
        }
        else
        {
            if (Input.GetKeyDown(MeleeKey))
            {
                MeleeAttack();
            }
            Anim.transform.localRotation = Quaternion.Euler(0,180,0);
            Weapons.SetActiveAnim(false);
            IsAttacking = false;
        }
    }

    void MeleeAttack()
    {
        //Expl.GetComponent<Scr_PAExplosion>().setRadius(ExplosionRadius);
        //Expl.transform.position = transform.position;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5);
        List<Transform> TransfromList = new List<Transform>();
        foreach (var hitCollider in hitColliders)
        {
            var CustomCollider = hitCollider.gameObject.GetComponent<CustomCollider>();
            if (CustomCollider != null)
            {
                if (!TransfromList.Contains(CustomCollider.transform.root))
                {  
                    CustomCollider.TakeDamage(75, CustomCollider.DamageType.Normal, -transform.forward);
                    TransfromList.Add(CustomCollider.transform.root);
                }
            }

            if (hitCollider.GetComponent<Script_InteractEvent>())
            {
                var hitEvent = hitCollider.GetComponent<Script_InteractEvent>();
                if (hitEvent.EventType == Script_InteractEvent.InteractEventType.OnHit)
                {
                    hitEvent.Interact();
                }
            }
        }


        MeeleTimer = MeeleTime;
        Anim.SetTrigger("melee");
        CameraEffects.RotateTo += new Vector3(0,90,-05);
        CameraEffects.ShakeTime += 0.5f;
        CameraEffects.ShakeAmplitude += 2;

        AS.PlayOneShot(MeeleSounds[Random.Range(0,MeeleSounds.Length)]);
    }
}
