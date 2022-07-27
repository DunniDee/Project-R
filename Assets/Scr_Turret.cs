using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Turret : MonoBehaviour
{
    [SerializeField] Transform PlayerTransform;
    [SerializeField] Transform TurretHead;
    [SerializeField] Transform Look;
    [SerializeField] GameObject Projectile;

    [SerializeField] Transform ShootPos;

    [SerializeField] float m_ShootTime;
    [SerializeField] float m_ShootTimer;

    [SerializeField] float TrackSpeed;

    AudioSource AS;

    Vector3 LastPos;

    [SerializeField] AudioClip ShootSound;
    // Start is called before the first frame update
    void Start()
    {
        AS = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = PlayerTransform.position - LastPos;
        Look.LookAt(PlayerTransform.position + velocity * 10);
        TurretHead.rotation = Quaternion.Lerp(TurretHead.rotation, Look.rotation, Time.deltaTime * TrackSpeed);

        if (m_ShootTimer > 0)
        {
            m_ShootTimer -= Time.deltaTime;
        }
        else
        {
            m_ShootTimer = m_ShootTime;

            for (int i = 0; i < 3; i++)
            {
                Invoke("Shoot", i * 0.15f);
            }
        }

        LastPos = PlayerTransform.position;
    }

    void Shoot()
    {
        GameObject Proj = ObjectPooler.Instance.GetObject(Projectile);
        Proj.transform.position = ShootPos.transform.forward;
        Proj.GetComponent<Rigidbody>().velocity = ShootPos.transform.forward  * 100;
        Proj.transform.position = ShootPos.position;
        Proj.transform.rotation = ShootPos.rotation;
        AS.PlayOneShot(ShootSound);
    }
}
