using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Credit : MonoBehaviour
{
    [Header("Internal Components")]
    public AudioSource audioSource;
    public AudioClip pickupAudio;

    [Header("Internal Properties")]
    public Transform Parent;
    public Transform Target;
    public float minSpeed;
    public float maxSpeed;

    public Vector2 ValueMinMax;

    private Vector3 Velocity;

    bool isFollowing = false;

    public float Value = 0;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Add Money
            OnPlayerCollided(other);
            PlayPickUpNoise();
            Destroy(Parent.gameObject, 0.25f);
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
            Parent.position = Vector3.SmoothDamp(Parent.position, new Vector3(Target.position.x, Target.position.y + 1, Target.position.z), ref Velocity, Time.deltaTime * Random.Range(minSpeed, maxSpeed));

        }
    }
    // Start is called before the first frame update
    public void Start()
    {
        audioSource = GetComponentInParent<AudioSource>();
    
        Target = GameObject.FindGameObjectWithTag("Player").transform;

        Value = Random.Range(ValueMinMax.x, ValueMinMax.y);
    }

    // Update is called once per frame
    public void Update()
    {

        FollowTarget();

    }
}
