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

    // public bool Damage(float _Damage, CustomCollider.DamageType DamageType, Vector3 force)
    // {
    //     Health -= _Damage;
    //     rigidBody.AddForce(force, ForceMode.Impulse);
    //     OnDeath();
    //     Debug.Log("Hit");

    //     return true;
    // }

        public bool Damage(float _Damage, CustomCollider.DamageType _DamageType,Vector3 _direction)
    {
        switch(_DamageType)
        {
            case CustomCollider.DamageType.Critical:
                Health -= _Damage * 2;
                // Scr_DamagePopupManager.Instance.DisplayDamagePopup((int)_Damage * 2, DamagePopupPos);
                // Scr_DamagePopupManager.Instance.CreateHealthOrb(this.gameObject.transform);
            break;
            
            case CustomCollider.DamageType.Normal:
                Health -= _Damage;
                // Scr_DamagePopupManager.Instance.DisplayDamagePopup((int)_Damage, DamagePopupPos);
            break;
        }

        if (Health <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
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
