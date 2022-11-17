using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Debugger : MonoBehaviour
{
    public KeyCode Keycode_NoClip;

    public bool isNoclipEnabled = false;

    float flySpeed = 10;
    public Camera defaultCam;
    public Camera defaulttransformCam;
    public GameObject playerObject;

    public bool shift = false;
    public bool ctrl = false;
    public float accelerationAmount = 30;
    public float accelerationRatio = 3;
    public float slowDownRatio = 0.2f;

    /// <summary>
    /// 
    /// </summary>
    public void ToggleNoClip()
    {
        if (Input.GetKeyDown(Keycode_NoClip))
        {
            switchCamera();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void NoClipUpdate()
    {
        if (isNoclipEnabled)
        {
            //use shift to speed up flight
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                shift = true;
                flySpeed *= accelerationRatio;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
            {
                shift = false;
                flySpeed /= accelerationRatio;
            }

            //use ctrl to slow up flight
            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            {
                ctrl = true;
                flySpeed *= slowDownRatio;
            }

            if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
            {
                ctrl = false;
                flySpeed /= slowDownRatio;
            }
            //
            if (Input.GetAxis("Vertical") != 0)
            {
                transform.Translate(Vector3.forward * flySpeed * Input.GetAxis("Vertical"));
            }


            if (Input.GetAxis("Horizontal") != 0)
            {
                transform.Translate(Vector3.right * flySpeed * Input.GetAxis("Horizontal"));
            }


            if (Input.GetKey(KeyCode.E))
            {
                transform.Translate(Vector3.up * flySpeed);
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                transform.Translate(Vector3.down * flySpeed);
            }
           

            if (Input.GetKeyDown(KeyCode.M))
                playerObject.transform.position = transform.position; //Moves the player to the flycam's position. Make sure not to just move the player's camera.

        }
    }
  
    /// <summary>
    /// 
    /// </summary>
    void switchCamera()
    {
        if (!isNoclipEnabled) //means it is currently disabled. code will enable the flycam. you can NOT use 'enabled' as boolean's name.
        {
            transform.position = defaultCam.transform.position; //moves the flycam to the defaultcam's position
            defaultCam.enabled = false;
            defaulttransformCam.enabled = false;
            playerObject.GetComponent<Scr_PlayerMotor>().enabled = false ;
            this.GetComponent<Camera>().enabled = true;
            GetComponentInChildren<Camera>().enabled = true;
            isNoclipEnabled = true;
        }
        else if (isNoclipEnabled) //if it is not disabled, it must be enabled. the function will disable the freefly camera this time.
        {
            this.GetComponent<Camera>().enabled = false;
            defaultCam.enabled = true;
            defaulttransformCam.enabled = true;
            playerObject.GetComponent<Scr_PlayerMotor>().enabled = true;
            GetComponentInChildren<Camera>().enabled = false;
            isNoclipEnabled = false;
        }
    }

    void KillPlayer()
    {
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            GetComponent<Scr_PlayerHealth>().TakeDamage(Mathf.Infinity, 0, 0);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       /* KillPlayer();
        ToggleNoClip();
        NoClipUpdate();*/
    }
}
