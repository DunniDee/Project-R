using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class decal_anim : MonoBehaviour
{
    public DecalProjector decal;
    public float fade = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Example4());
    }

    // Update is called once per frame
    void Update()
    {
        decal.fadeFactor -= Time.deltaTime * fade;
    }

    IEnumerator Example4()
    {

        yield return new WaitForSeconds(5);

        StartCoroutine(Example4());

        decal.fadeFactor = 1f;
    }
}
