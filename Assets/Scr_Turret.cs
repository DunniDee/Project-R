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

    [SerializeField] bool InRange;
    [SerializeField] bool InSight;

    [SerializeField] bool IsArmed;
    bool WasArmed;

    AudioSource AS;

    Vector3 LastPos;

    [SerializeField] LayerMask Mask;

    [SerializeField] AudioClip ShootSound;
     [SerializeField] AudioClip AlarmSound;
    // Start is called before the first frame update
    void Start()
    {
        AS = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        IsArmed = LineOfSight() && InRange;
        if (IsArmed)
        {
            if (!WasArmed)
            {
                AS.PlayOneShot(AlarmSound);
            }

            IsArmed = true;
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
        WasArmed = IsArmed;
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

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            InRange = true;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            InRange = false;
        }
    }

    private bool LineOfSight()
    {
        RaycastHit Hit;
        if (Physics.Raycast(TurretHead.position, PlayerTransform.position - TurretHead.position, out Hit, 100,Mask,QueryTriggerInteraction.Ignore))
        {
            if (Hit.transform.CompareTag("Player"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
