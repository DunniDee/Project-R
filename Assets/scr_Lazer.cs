using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Lazer : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    bool hitBlocked;

    RaycastHit Hit;
    public float maxLaserDistance = 10.0f;
    float damagePerTick = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

    }
    private void OnValidate()
    {
        if (lineRenderer)
        {
            lineRenderer.SetPosition(0, transform.position);
            if (hitBlocked == true) // we've hit something, so our line renderer end point should stop here
            {
                lineRenderer.SetPosition(1, Hit.point);
            }
            else
            {
                lineRenderer.SetPosition(1, transform.position + transform.forward * maxLaserDistance);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (lineRenderer)
        {
            lineRenderer.SetPosition(0, transform.position);
            if (hitBlocked == true) // we've hit something, so our line renderer end point should stop here
            {
                lineRenderer.SetPosition(1, Hit.point);
            }
            else
            {
                lineRenderer.SetPosition(1, transform.position + transform.forward * maxLaserDistance);
            }
        }

        if (Physics.Raycast(transform.position, transform.forward, out Hit, maxLaserDistance))
        {
            hitBlocked = true;
            if (Hit.transform.CompareTag("Player"))
            {
                var playerhealth = Hit.transform.GetComponent<Scr_PlayerHealth>();
                playerhealth.TakeDamage(damagePerTick);
            }
        }
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * maxLaserDistance);
    }
}
