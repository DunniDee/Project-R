using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_DesctruableObject : MonoBehaviour, IDamageable
{
    public float Health = 100.0f;
    public bool isDestroyed = false;
    public Rigidbody rigidBody;
    public Collider Colldier;
    public MeshRenderer renderer;

    public AudioSource audiosource;
    public AudioClip DestructSound;
    public delegate void OnDestructDelegate();
    public event OnDestructDelegate OnDestructEvent;

    public bool Damage(float _Damage, CustomCollider.DamageType DamageType, Vector3 force)
    {
        Health -= _Damage;
        rigidBody.AddForce(force, ForceMode.Impulse);
        OnDeath();
        return true;
    }

    private void OnDeath()
    {

        if (Health <= 0.0f)
        {
            isDestroyed = true;
            audiosource.PlayOneShot(DestructSound);
            Colldier.enabled = false;
            renderer.enabled = false;
            if (OnDestructEvent != null)
            {
                OnDestructEvent();
            }
            else
            {
                Destroy(gameObject);
                
            }

        }
    }

   

   

    // Start is called before the first frame update
    public void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();
    }
}
