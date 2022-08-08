using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
public class Scr_MovingPlatform : MonoBehaviour
{
    public List<Vector3> TargetPosition;
    public MeshCollider Colldier;

    public bool isMoving = false;
    public float WaitTIme = 3.0f;
    public float curWaitTimer = 0.0f;

    public float MoveSpeed = 2.0f;
    public float Acceleration = 1.0f;
    public void MovePlatform()
    {
        if (TargetPosition.Count > 0)
        { 
            //transform.
        }
    }

    public IEnumerator MovePlatformCoroutine()
    {
        yield return new WaitForSeconds(WaitTIme);
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        
    }
}
