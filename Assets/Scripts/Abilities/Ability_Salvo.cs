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
    private void Start()
    {
        Look = transform.GetChild(0);
    }

    protected override void OnAbilityStart()
    {
        base.OnAbilityStart();
        Salvo();
    }

    void Salvo()
    {
        for (int i = 0; i < BurstCount; i++)
        {
            Invoke("Shoot", BurstSpeed * i);
        }
    }

    void Shoot()
    {
        GameObject Proj = ObjectPooler.Instance.GetObject(Grenade);

        Proj.transform.position = Look.position;
        Proj.transform.rotation = Look.rotation;

        Scr_RCSplashProjectile SpTemp = Proj.GetComponent<Scr_RCSplashProjectile>();
        SpTemp.SetDamage(Damage);
        SpTemp.SetSpeed(Force);
        SpTemp.SetRadius(Radius);
        SpTemp.SetlifeTime(5);
    }
}
