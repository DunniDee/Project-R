using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scr_DamagePopup : MonoBehaviour
{

    private TMP_Text text; // Access text mesh properties
    private Color textColor; // change alpha -> fade out
    private Transform playerTransform; // look towards the player..

    private float dissapearTimer = 0.5f;
    private float fadeOutSpeed = 5f;
    private Vector3 MoveDir = Vector3.zero;
    private float speed = 5f;

    public void Init(int _Damage)
    {
       
        text = GetComponentInChildren<TMP_Text>();
        playerTransform = Camera.main.transform;
       
        textColor = text.color;
        MoveDir = new Vector3(Random.Range(-1, 1), 1, 0);
        
        text.SetText(_Damage.ToString());
        transform.localPosition = new Vector3(transform.localPosition.x + Random.Range(-1, 1), transform.localPosition.y + Random.Range(-1, 1), 0.0f);
    }
    private void LateUpdate()
    {
        transform.LookAt(2 * transform.position - playerTransform.position);
        transform.position += MoveDir * speed * Time.deltaTime;
        dissapearTimer -= Time.deltaTime;
        if (dissapearTimer <= 0f)
        {
            textColor.a -= fadeOutSpeed * Time.deltaTime;
            text.color = textColor;
            if (textColor.a <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
   
}
