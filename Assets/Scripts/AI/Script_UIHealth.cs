using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Script_UIHealth : MonoBehaviour
{
    public Transform target;
    public Image HealthBarFill;
    public Image HealthBarBG;
    public Slider HealthSlider;

    // Start is called before the first frame update
    public void Start()
    {
        HealthSlider = GetComponent<Slider>();
    }

    
    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 Direction = (target.position - Camera.main.transform.position).normalized;
        bool isBehind  = Vector3.Dot(Camera.main.transform.forward, Direction) <= 0.0f;
        HealthBarBG.enabled = !isBehind;
        HealthBarFill.enabled = !isBehind;

        transform.position = Camera.main.WorldToScreenPoint(new Vector3(target.position.x, target.position.y + 2, target.position.z));
    }

    void Update(){
        

    }
}
