using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scr_skipcutscene : MonoBehaviour
{
    public float Time = 27f;

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene("MenuLevelScene");
        }
    }

    void Start()
    {

        StartCoroutine(Example4());

    }

    IEnumerator Example4()
    {

        yield return new WaitForSeconds(Time);

        SceneManager.LoadScene("MenuLevelScene");


    }

}


