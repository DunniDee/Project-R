using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_DeathParticles : MonoBehaviour
{
    [SerializeField] ParticleSystem[] ParticleSystems;
    public void PlayParticles()
    {
        foreach (var p in ParticleSystems)
        {
            p.Play();
        }
    }
}
