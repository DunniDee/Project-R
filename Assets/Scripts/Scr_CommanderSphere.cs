using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_CommanderSphere : MonoBehaviour
{
    MeshRenderer meshrenderer;
    [SerializeField]
    GameObject ps;
    [SerializeField]
    List<GameObject> psList;
    
    // String Path to Particle System example "Resources/Particles/..."
    public string ParticleResourcePath;

    public void SetMeshRenderActive(bool active)
    {
        meshrenderer.enabled = active;        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.GetComponentInParent<Script_BaseAI>())
        {

            Debug.Log("Agent Entered the Arena");
            var agent = other.GetComponentInParent<Script_BaseAI>();
            if (agent.GetComponentInChildren<ParticleSystem>())
            {
                return;
            }
            agent.StatDamage = agent.Config.projectileDamage * 0.5f;
            var newParticlesystem = Instantiate(ps, agent.transform);
            psList.Add(newParticlesystem);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<Script_BaseAI>())
        {
            var agent = other.GetComponentInParent<Script_BaseAI>();
            agent.StatDamage = agent.Config.projectileDamage * 0.5f;
            foreach (GameObject ps in psList)
            {
                if (psList.Count == 0)
                {
                    break;
                }
                if (ps == agent.GetComponentInChildren<ParticleSystem>().gameObject)
                {
                    psList.Remove(ps);
                    Destroy(agent.GetComponentInChildren<ParticleSystem>().gameObject);
                }
            }
        }
    }

    private void Start()
    {
        meshrenderer = GetComponent<MeshRenderer>();
        ps = Resources.Load(ParticleResourcePath) as GameObject;

    }
}
