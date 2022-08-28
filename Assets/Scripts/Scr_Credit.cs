using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script Owner: Ashley Rickit

public class Scr_Credit : MonoBehaviour
{
    [Header("Internal Components")]
    public AudioSource audioSource;
    public AudioClip pickupAudio;

    [Header("Internal Properties")]
    public Transform Target;
    public float minSpeed;
    public float maxSpeed;

    public AnimationCurve SpeedCurve;

    public Vector2 ValueMinMax;

    bool isFollowing = true;

    public float Value = 0;

    float lifeTime;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Add Money
            OnPlayerCollided(other);
            PlayPickUpNoise();
            Script_PlayerStatManager.Instance.Credits += Value;
            Destroy(transform.gameObject, 0.25f);
        }
    }
    protected virtual void OnPlayerCollided(Collider other) {
        Debug.Log("Collided With Player");
    }

    protected void PlayPickUpNoise()
    {
        audioSource.PlayOneShot(pickupAudio);
    }
    public void StartFollowing()
    {
        isFollowing = true;
    }

    private void FollowTarget()
    {
        
        if (isFollowing)
        {
            lifeTime += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position + (Vector3.up * 2), Target.position, lifeTime);
        }
    }

    // Start is called before the first frame update
    public void Start()
    {
        audioSource = GetComponentInParent<AudioSource>();
    
        Target = GameObject.FindGameObjectWithTag("Player").transform;

        //Value = Random.Range(ValueMinMax.x, ValueMinMax.y);
    }

    // Update is called once per frame
    public void Update()
    {
        
        FollowTarget();

    }
}
